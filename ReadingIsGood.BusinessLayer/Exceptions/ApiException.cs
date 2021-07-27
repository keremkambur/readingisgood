using System;
using System.Net;

namespace ReadingIsGood.BusinessLayer.Exceptions
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public ApiException(HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : this(null, null, statusCode)
        {
        }

        public ApiException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : this(message, null, statusCode)
        {
        }

        public ApiException(string message, Exception innerException, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message, innerException)
        {
            this.StatusCode = statusCode;
        }
    }
}