namespace Commons.HttpClientService.Models
{
    [System.Serializable]
    public class HttpResponseModel
    {
        public List<HttpHeaderModel> Headers { get; set; }
        public string Response { get; set; }
        public ServiceErrorModel ServiceError { get; set; }
    }

}
