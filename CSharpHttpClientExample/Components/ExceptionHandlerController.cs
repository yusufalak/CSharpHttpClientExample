using Commons.Exceptions;
using Commons.HttpClientService.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Commons.Components
{
    [ApiController]
    public class ExceptionHandlerController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;

            BaseResponseModel errorResponse = CreateBaseResponseModelFromException(exception);

            var statusCode = StatusCodes.Status202Accepted; // Accepted will be retuned default in order to consume http response entity without catching exception on client side.
            return StatusCode(statusCode, errorResponse);
        }

        private BaseResponseModel CreateBaseResponseModelFromException(System.Exception exception)
        {
            var errorCode = ErrorCodes.UNKNOWN_ERROR;
            List<SingleMessageArgument> arguments = null;

            if (exception is SubChannelException subchannelException)
            {
                errorCode = subchannelException.ErrorCode;
                arguments = subchannelException.Arguments;
            }
            else if (exception is HttpRequestException httpException)
            {
                errorCode = ErrorCodes.URL_CONNECTION_ERROR;
                arguments = new List<SingleMessageArgument> { new SingleMessageArgument(new string[] { httpException.Message }) };
            }

            return new BaseResponseModel
            {
                ResponseTime = System.DateTime.Now,
                ServiceError = new ServiceErrorModel()
                {
                    Code = errorCode.Code,
                    Arguments = arguments,
                    Message = errorCode.Message
                }
            };
        }
    }
}
