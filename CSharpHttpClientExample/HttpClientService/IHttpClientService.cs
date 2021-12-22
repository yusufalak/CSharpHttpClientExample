using Commons.HttpClientService.Models;

namespace Commons.HttpClientService
{
    public interface IHttpClientService
    {
        HttpResponseModel Execute(HttpRequestModel httpRequest);
    }
}
