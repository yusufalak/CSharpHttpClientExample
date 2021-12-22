using Commons.Extensions;
using Commons.HttpClientService;
using Microsoft.AspNetCore.Mvc;

namespace Commons.Services.Channel.Rest
{
    [ApiController]
    [Route("api")]
    public class RestChannel : ControllerBase
    {
        private readonly IHttpClientService httpClientService;
        private readonly IHttpClientService blockingHttpClientService;

        public RestChannel(ServiceFactory<IHttpClientService> _serviceFactoryHttpClientService)
        {
            this.httpClientService = _serviceFactoryHttpClientService.Resolve(HttpClientConfigurationNames.COMMONS_HTTP_CLIENT_SERVICE);
            this.blockingHttpClientService = _serviceFactoryHttpClientService.Resolve(HttpClientConfigurationNames.COMMONS_BLOCKING_HTTP_CLIENT_SERVICE);
        }

        [HttpGet("stat")]
        public IActionResult Stat()
        {
            var info = "OK";
            return Ok(info);
        }

        [HttpPost("httpClient")]
        public IActionResult PostHttpClient(HttpClientService.Models.HttpRequestModel httpRequest)
        {
            var channelResponse = httpClientService.Execute(httpRequest);
            return Ok(channelResponse);
        }

        [HttpPost("blockingHttpClient")]
        public IActionResult PostBlockingHttpClient([FromBody] HttpClientService.Models.HttpRequestModel httpRequest)
        {
            var channelResponse = blockingHttpClientService.Execute(httpRequest);
            return Ok(channelResponse);
        }



    }
}
