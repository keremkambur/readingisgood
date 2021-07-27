using System.Net;

namespace ReadingIsGood.BusinessLayer.Exceptions
{
    public class ForbiddenAccessException : ApiException
    {
        public ForbiddenAccessException() : base(HttpStatusCode.Forbidden)
        {
        }

        public ForbiddenAccessException(string message) : base(message, HttpStatusCode.Forbidden)
        {
        }

        public ForbiddenAccessException(string message, System.Exception innerException) : base(message, innerException, HttpStatusCode.Forbidden)
        {
        }
    }
}