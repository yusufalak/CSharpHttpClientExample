using Commons.HttpClientService.Models;
using System.Text;

namespace Commons.Log
{
    public static class LoggingUtil
    {
        public static void LogRequestResponseDebug(Logger log, HttpRequestModel httpParam, String responseBody, Exception exception)
        {
            log.Debug(CreateMessage(httpParam, responseBody, exception));
        }

        public static void LogRequestResponseDebug(Logger log, HttpRequestModel httpParam, String responseBody)
        {
            log.Debug(CreateMessage(httpParam, responseBody, null));
        }

        public static void LogRequestResponseInfo(Logger log, HttpRequestModel httpParam, String responseBody, Exception exception)
        {
            log.Info(CreateMessage(httpParam, responseBody, exception));
        }

        public static void LogRequestResponseInfo(Logger log, HttpRequestModel httpParam, String responseBody)
        {
            log.Info(CreateMessage(httpParam, responseBody, null));
        }

        public static void LogRequestResponseWarn(Logger log, HttpRequestModel httpParam, String responseBody, Exception exception)
        {
            log.Warn(CreateMessage(httpParam, responseBody, exception));
        }

        public static void LogRequestResponseWarn(Logger log, HttpRequestModel httpParam, String responseBody)
        {
            log.Warn(CreateMessage(httpParam, responseBody, null));
        }

        public static void LogRequestResponseError(Logger log, HttpRequestModel httpParam, String responseBody, Exception exception)
        {
            log.Error(CreateMessage(httpParam, responseBody, exception));
        }

        public static void LogRequestResponseError(Logger log, HttpRequestModel httpParam, String responseBody)
        {
            log.Error(CreateMessage(httpParam, responseBody, null));
        }

        private static String CreateMessage(HttpRequestModel httpParam, String responseBody, Exception exception)
        {

            StringBuilder buf = new StringBuilder().Append("\n");

            buf.Append("URL: " + httpParam.URL).Append("\n");

            if (!string.IsNullOrEmpty(httpParam.AuthorizationToken))
            {
                buf.Append("AuthorizationToken: " + httpParam.AuthorizationToken).Append("\n");
            }

            if (!string.IsNullOrEmpty(httpParam.SoapAction))
            {
                buf.Append("SoapAction: " + httpParam.SoapAction).Append("\n");
            }

            if (!string.IsNullOrEmpty(httpParam.ServiceName))
            {
                buf.Append("ServiceName: " + httpParam.ServiceName).Append("\n");
            }

            List<HttpHeaderModel> additionalHeaders = httpParam.Headers;
            if (additionalHeaders != null && additionalHeaders.Count > 0)
            {
                buf.Append("Headers:").Append("\n");
                additionalHeaders.ForEach(h => { buf.Append(h.Name + ":" + h.Value).Append("\n"); });
            }

            Dictionary<string, string> urlParams = httpParam.RequestParameters;
            if (urlParams != null && urlParams.Count > 0)
            {
                buf.Append("Parameters:").Append("\n");

                foreach (var item in urlParams)
                {
                    buf.Append(item.Key + ":" + item.Value).Append("\n");
                }
            }

            buf.Append("RequestBody:").Append("\n").Append(httpParam.RequestBody).Append("\n");

            if (responseBody != null)
            {
                buf.Append("ResponseBody:").Append("\n").Append(responseBody).Append("\n");
            }

            if (exception != null)
            {
                buf.Append("Exception:").Append("\n").Append(exception.Message).Append("\n");
            }

            return buf.ToString();
        }
    }
}
