
using Commons.Exceptions;
using Commons.HttpClientService.Models;
using Commons.Log;
using System.Text;

namespace Commons.ExceptionService
{
    public class ExceptionService : IExceptionService
    {
        private readonly Logger LOG = Logger.GetInstance(typeof(ExceptionService));

        public void CheckError(List<ServiceErrorModel> errors)
        {
            if (errors == null || errors.Count <= 0)
                return;

            StringBuilder errorBuf = new StringBuilder();
            foreach (var error in errors)
            {
                errorBuf.Append(error.Code).Append(".").Append(error.Message).Append("\n");
            }

            throw Handle(errorBuf.ToString());
        }

        public SubChannelException Handle(string exceptionMessage)
        {
            if (exceptionMessage.Contains("No such host is known"))
                return new SubChannelException(ErrorCodes.URL_CONNECTION_ERROR);

            return new SubChannelException(ErrorCodes.UNKNOWN_ERROR, exceptionMessage);
        }

        public SubChannelException CreateException(ErrorCodes error)
        {
            return new SubChannelException(error);
        }

        public SubChannelException CreateException(ErrorCodes error, params string[] args)
        {
            if (args == null || args.Length <= 0)
                return new SubChannelException(error);

            return new SubChannelException(error, args);
        }

        public SubChannelException HandleFinalException(Exception exception)
        {
            LOG.Error($"::handleFinalException ex:{exception.Message}", exception);

            if (exception is SubChannelException subChannelException)
                throw subChannelException;

            throw Handle(exception.Message);
        }

    }
}
