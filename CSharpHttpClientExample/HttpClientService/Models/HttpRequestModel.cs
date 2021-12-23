namespace Commons.HttpClientService.Models
{
    [System.Serializable]
    public class HttpRequestModel
    {
        public string? URL { get; set; } = null;

        public string? Method { get; set; } = null;

        public string? ContentType { get; set; } = null;

        public string? Charset { get; set; } = null;

        public List<HttpHeaderModel>? Headers { get; set; } = null;

        public Dictionary<string, string>? RequestParameters { get; set; } = null;

        public Dictionary<string, string>? FormData { get; set; } = null;

        public string? AuthorizationToken { get; set; } = null;

        public string? SoapAction { get; set; } = null;

        public string? RequestBody { get; set; } = null;

        public string? ServiceName { get; set; } = null;

        public bool? RetryOnError { get; set; } = false;

        public bool? UseGzip { get; set; } = false;

        public bool? GetBodyElement { get; set; } = false;

        public int? RetryCount { get; set; } = 5;
    }
}
