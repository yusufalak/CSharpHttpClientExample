
using Commons.Exceptions;
using Commons.HttpClientService.Models;

namespace Commons.ExceptionService
{
    public interface IExceptionService
    {
        void CheckError(List<ServiceErrorModel> errors);
        SubChannelException Handle(string exceptionMessage);
        SubChannelException CreateException(ErrorCodes error);
        SubChannelException CreateException(ErrorCodes error, params string[] args);
        SubChannelException HandleFinalException(Exception exception);
    }
}