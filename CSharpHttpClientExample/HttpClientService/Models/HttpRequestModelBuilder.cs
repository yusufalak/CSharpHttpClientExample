namespace Commons.HttpClientService.Models
{
    [System.Serializable]
    public static class HttpRequestModelBuilder
    {

        public static HttpRequestModel Post()
        {
            return new HttpRequestModel
            {
                Method = HttpMethod.Post
            };
        }

        public static HttpRequestModel Get()
        {
            return new HttpRequestModel
            {
                Method = HttpMethod.Get
            };
        }

        public static HttpRequestModel WithUrl(this HttpRequestModel httpRequest, string url)
        {
            httpRequest.URL = url;
            return httpRequest;
        }

        public static HttpRequestModel WithContentType(this HttpRequestModel httpRequest, string contentType)
        {
            httpRequest.ContentType = contentType;
            return httpRequest;
        }

        public static HttpRequestModel WithCharset(this HttpRequestModel httpRequest, string charset)
        {
            httpRequest.Charset = charset;
            return httpRequest;
        }

        public static HttpRequestModel WithCharsetUtf8(this HttpRequestModel httpRequest)
        {
            httpRequest.Charset = "UTF8";
            return httpRequest;
        }

        public static HttpRequestModel WitHeaders(this HttpRequestModel httpRequest, List<HttpHeaderModel> headers)
        {
            httpRequest.Headers = headers;
            return httpRequest;
        }

        public static HttpRequestModel AddHeader(this HttpRequestModel httpRequest, HttpHeaderModel header)
        {
            if (httpRequest.Headers == null)
            {
                httpRequest.Headers = new List<HttpHeaderModel>();
            }

            httpRequest.Headers.Add(header);
            return httpRequest;
        }

        public static HttpRequestModel AddHeader(this HttpRequestModel httpRequest, string name, string value)
        {
            if (httpRequest.Headers == null)
            {
                httpRequest.Headers = new List<HttpHeaderModel>();
            }

            httpRequest.Headers.Add(new HttpHeaderModel(name, value));
            return httpRequest;
        }

        public static HttpRequestModel WitRequestParams(this HttpRequestModel httpRequest, Dictionary<string, string> parameters)
        {
            httpRequest.RequestParameters = parameters;
            return httpRequest;
        }

        public static HttpRequestModel AddRequestParam(this HttpRequestModel httpRequest, string paramName, string paramValue)
        {
            if (httpRequest.RequestParameters == null)
            {
                httpRequest.RequestParameters = new Dictionary<string, string>();
            }

            httpRequest.RequestParameters.Add(paramName, paramValue);
            return httpRequest;
        }

        public static HttpRequestModel AddRequestParam(this HttpRequestModel httpRequest, (string, string) param)
        {
            if (httpRequest.RequestParameters == null)
            {
                httpRequest.RequestParameters = new Dictionary<string, string>();
            }

            httpRequest.RequestParameters.Add(param.Item1, param.Item2);
            return httpRequest;
        }

        public static HttpRequestModel WithAuthorization(this HttpRequestModel httpRequest, string authorizationToken)
        {
            httpRequest.AuthorizationToken = authorizationToken;
            return httpRequest;
        }

        public static HttpRequestModel WithSoapAction(this HttpRequestModel httpRequest, string soapAction)
        {
            httpRequest.SoapAction = soapAction;
            return httpRequest;
        }

        public static HttpRequestModel WithRequestBody(this HttpRequestModel httpRequest, string requestBody)
        {
            httpRequest.RequestBody = requestBody;
            return httpRequest;
        }
        public static HttpRequestModel WithServiceName(this HttpRequestModel httpRequest, string serviceName)
        {
            httpRequest.ServiceName = serviceName;
            return httpRequest;
        }

        public static HttpRequestModel WithRetryOnError(this HttpRequestModel httpRequest, bool retryOnError)
        {
            httpRequest.RetryOnError = retryOnError;
            return httpRequest;
        }

        public static HttpRequestModel WithUseGzip(this HttpRequestModel httpRequest, bool useGzip)
        {
            httpRequest.UseGzip = useGzip;
            return httpRequest;
        }

        public static HttpRequestModel WithGetBodyElement(this HttpRequestModel httpRequest, bool getBodyElement)
        {
            httpRequest.GetBodyElement = getBodyElement;
            return httpRequest;
        }

        public static HttpRequestModel WithRetryCount(this HttpRequestModel httpRequest, int retryCount)
        {
            httpRequest.RetryCount = retryCount;
            return httpRequest;
        }

    }
}
