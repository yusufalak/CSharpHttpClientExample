using Commons.Interceptors;

namespace Commons.Extensions
{
    public static class HttpRequestMiddlewareExtensions
    {
        public static IApplicationBuilder UseSubChannelHttpRequestMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpRequestInterceptorMiddleware>();
        }

        public static IApplicationBuilder UseSubChannelExceptionHandler(
           this IApplicationBuilder builder)
        {
            return builder.UseExceptionHandler("/error");
        }
    }
}
