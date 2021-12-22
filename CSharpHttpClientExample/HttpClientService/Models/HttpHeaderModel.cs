namespace Commons.HttpClientService.Models
{
    [System.Serializable]
    public class HttpHeaderModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public HttpHeaderModel(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
