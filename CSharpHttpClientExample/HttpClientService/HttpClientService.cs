using Commons.Exceptions;
using Commons.ExceptionService;
using Commons.HttpClientService.Models;
using Commons.Log;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace Commons.HttpClientService
{
    public class HttpClientService : IHttpClientService
    {
        private static readonly Logger LOG = Logger.GetInstance(typeof(HttpClientService));

        private static readonly string GZIP = "gzip";
        private static readonly string CONTENT_ENCODING = "content-encoding";

        protected readonly HttpClient _httpClient;
        protected readonly IExceptionService _exceptionService;

        public HttpClientService(IExceptionService exceptionService, IHttpClientFactory _httpClientfactory, bool blocking)
        {
            this._exceptionService = exceptionService;
            if (blocking)
            {
                this._httpClient = _httpClientfactory.CreateClient("blocking");
            }
            else
            {
                this._httpClient = _httpClientfactory.CreateClient("default");
            }
        }

        public virtual HttpResponseModel Execute(HttpRequestModel httpRequest)
        {
            try
            {
                Task<HttpResponseModel> internalResponse = ExecuteInternal(httpRequest);
                HttpResponseModel responseBody = internalResponse.Result;
                LoggingUtil.LogRequestResponseInfo(LOG, httpRequest, responseBody.Response);
                return responseBody;
            }
            catch (Exception exception)
            {
                LoggingUtil.LogRequestResponseError(LOG, httpRequest, exception.Message, exception);
                throw _exceptionService.HandleFinalException(exception);
            }


        }

        private async Task<HttpResponseModel> ExecuteInternal(HttpRequestModel httpRequest)
        {
            if (httpRequest == null)
                throw _exceptionService.CreateException(ErrorCodes.MISSING_MANDATORY_FIELD, "HttpRequestModel");

            bool retryOnError = httpRequest.RetryOnError != null ? httpRequest.RetryOnError.Value : false;
            int retryCount = httpRequest.RetryCount != null ? httpRequest.RetryCount.Value : 0;

            HttpRequestMessage httpRequestMessage = CreateHttpRequestModel(httpRequest);

            int count = 0;
            do
            {
                count++;
                try
                {
                    HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);
                    return await CreateHttpResponseModel(httpResponseMessage);
                }
                catch (HttpRequestException)
                {
                    if (!retryOnError || retryCount > count) { throw; }
                }

            } while (retryOnError && retryCount < count);

            string errorMessage = "Unable to parse service response!";
            throw new SubChannelException(ErrorCodes.URL_CONNECTION_ERROR, errorMessage);
        }

        private async Task<HttpResponseModel> CreateHttpResponseModel(HttpResponseMessage response)
        {
            HttpContent entityContent = response.Content;
            HttpResponseHeaders responseHeaders = response.Headers;
            HttpContentHeaders contentHeaders = entityContent.Headers;
            List<HttpHeaderModel> headers = CreateHeaders(responseHeaders, contentHeaders);

            string responseBody = null;
            bool gzipFound = headers.Any(h => h.Name.ToLower().Trim().Equals(CONTENT_ENCODING) && h.Value.ToLower().Trim().Equals(GZIP));
            if (gzipFound)
            {
                var entity = await entityContent.ReadAsStreamAsync();
                responseBody = DecompressResponse(entity);
            }
            else
            {
                responseBody = await entityContent.ReadAsStringAsync();
            }

            return new HttpResponseModel
            {
                Headers = headers,
                Response = responseBody
            };
        }

        private string DecompressResponse(Stream responseStream)
        {
            using (var gzipStream = new GZipStream(responseStream, CompressionMode.Decompress))
            {
                using (var destinationStream = new MemoryStream())
                {
                    gzipStream.CopyTo(destinationStream);
                    return Encoding.UTF8.GetString(destinationStream.ToArray());
                }
            }
        }

        private List<HttpHeaderModel> CreateHeaders(HttpResponseHeaders headers, HttpContentHeaders contentHeaders)
        {
            IEnumerable<HttpHeaderModel> responseHttpHeaders = headers.Select(header => new HttpHeaderModel(header.Key, header.Value.First()));
            IEnumerable<HttpHeaderModel> contentHttpHeaders = contentHeaders.Select(header => new HttpHeaderModel(header.Key, header.Value.First()));
            return Enumerable.Concat(responseHttpHeaders, contentHttpHeaders).ToList();
        }

        private HttpRequestMessage CreateHttpRequestModel(HttpRequestModel httpRequest)
        {
            var builder = new UriBuilder(httpRequest.URL)
            {
                Port = -1
            };

            AddQueryParams(httpRequest.RequestParameters, builder);

            var url = builder.ToString();
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpRequest.Method, url);

            if (httpRequest.UseGzip.Value)
            {
                httpRequestMessage.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue(GZIP));
            }

            if (!string.IsNullOrEmpty(httpRequest.SoapAction))
            {
                httpRequestMessage.Headers.Add("SOAPAction", httpRequest.SoapAction);
            }

            if (!string.IsNullOrEmpty(httpRequest.AuthorizationToken))
            {
                httpRequestMessage.Headers.Add("Authorization", httpRequest.AuthorizationToken);
            }

            if (httpRequest.Headers != null)
            {
                httpRequest.Headers.ForEach(hdr => httpRequestMessage.Headers.Add(hdr.Name, hdr.Value));
            }

            httpRequestMessage.Headers.Add("Accept", httpRequest.ContentType);

            if (httpRequest.Method != HttpMethod.Get)
            {
                if (httpRequest.FormData != null && httpRequest.FormData.Count > 0)
                {
                    httpRequestMessage.Content = new FormUrlEncodedContent(httpRequest.FormData);
                }
                else
                {
                    System.Text.Encoding encoding = FindEncoding(httpRequest.Charset);
                    StringContent stringContent = new StringContent(httpRequest.RequestBody, encoding, httpRequest.ContentType);
                    httpRequestMessage.Content = stringContent;
                }
            }

            return httpRequestMessage;
        }

        private Encoding FindEncoding(string charset)
        {
            if (string.IsNullOrEmpty(charset))
                return Encoding.UTF8;

            string type = charset.ToLower().Replace("-", "").Trim();

            if ("utf8".Equals(type))
                return Encoding.UTF8;

            return Encoding.UTF8;
        }

        private void AddQueryParams(Dictionary<string, string> requestParams, UriBuilder builder)
        {
            if (requestParams == null || requestParams.Count == 0) { return; }

            var query = HttpUtility.ParseQueryString(builder.Query);
            foreach (var item in requestParams)
            {
                query[item.Key] = item.Value;
            }
            builder.Query = query.ToString();
        }
    }
}
