namespace Commons.HttpClientService.Models
{
    public class BaseResponseModel
    {
        public DateTime ResponseTime { get; set; }
        public long ExecutionTime { get; set; }
        public ServiceErrorModel ServiceError { get; set; }

    }
}