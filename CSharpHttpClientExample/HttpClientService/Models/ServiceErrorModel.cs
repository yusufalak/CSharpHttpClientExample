using Commons.Exceptions;

namespace Commons.HttpClientService.Models
{
    [System.Serializable]
    public class ServiceErrorModel
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SingleMessageArgument> Arguments { get; set; }
    }
}