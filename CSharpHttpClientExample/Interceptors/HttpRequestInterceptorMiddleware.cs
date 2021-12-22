using System.Text.Json;

namespace Commons.Interceptors
{
    public class HttpRequestInterceptorMiddleware
    {
        private readonly object HTTP_REQUEST = "http-request";
        private readonly RequestDelegate _next;

        public HttpRequestInterceptorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            Stream body = context.Request.Body;
            if (body.Length > 0)
            {
                using var reader = new StreamReader(body);
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                var rawMessage = reader.ReadToEnd();
                object request = JsonSerializer.Deserialize<object>(rawMessage);
                context.Items.Add(HTTP_REQUEST, request);
            }

            await _next(context);
        }
    }
}
