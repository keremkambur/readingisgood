using System;
using System.Collections.Generic;
using System.Net;
using ReadingIsGood.BusinessLayer.Exceptions;
using ReadingIsGood.BusinessLayer.ResponseModels.Base;
using ReadingIsGood.EntityLayer.Enum;

namespace ReadingIsGood.BusinessLayer.Extensions
{
    public static class IResponseExtensions
    {
        public static void SetError(this IResponse response, Exception ex)
        {
            response.Status = (int)HttpStatusCode.InternalServerError;

            var errorMessage = ex.Message + " RequestId [" + response.RequestId + "]";
            object payload = null;
            ErrorCodes code;

            switch (ex)
            {
                case ForbiddenAccessException castForbidden:
                    response.Status = (int)castForbidden.StatusCode;
                    code = ErrorCodes.Forbidden;
                    break;

                case InternalServerException _:
                    errorMessage = "There was an internal error, please contact to technical support. RequestId [" + response.RequestId + "]";
                    response.Status = (int)HttpStatusCode.InternalServerError;
                    code = ErrorCodes.Internal;
                    break;

                case RequestParameterException requestParameterException:
                    response.Status = (int)requestParameterException.StatusCode;
                    code            = ErrorCodes.BadRequest;
                    errorMessage = requestParameterException.AggregatedErrorMessage;
                    payload = requestParameterException.AggregatedErrorPayload;
                    break;

                case ApiException cast:
                    response.Status = (int)cast.StatusCode;
                    code = ErrorCodes.Internal;
                    break;

                case KeyNotFoundException _:
                    response.Status = (int)HttpStatusCode.BadRequest;
                    errorMessage = ex.Message;
                    code = ErrorCodes.Internal;
                    break;

                case OperationCanceledException _:
                    response.Status = (int)HttpStatusCode.NoContent;
                    errorMessage = "Operation canceled.";
                    code = ErrorCodes.RequestCanceled;
                    break;

                case ArgumentNullException _:
                    response.Status = (int)HttpStatusCode.BadRequest;
                    code = ErrorCodes.BadRequest;
                    break;

                case UnauthorizedAccessException _:
                    response.Status = (int)HttpStatusCode.Unauthorized;
                    code = ErrorCodes.Unauthorized;
                    break;

                default:
                    errorMessage = "There was an internal error, please contact to technical support. RequestId [" + response.RequestId + "]";
                    code = ErrorCodes.Unknown;
                    break;
            }

            response.Error = new ErrorResponse
            {
                Payload = payload,
                Code = code,
                Message = errorMessage
            };
        }
    }
}