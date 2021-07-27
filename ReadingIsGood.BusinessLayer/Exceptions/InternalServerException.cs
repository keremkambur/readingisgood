using System;

namespace ReadingIsGood.BusinessLayer.Exceptions
{
    public class InternalServerException : ApiException
    {
        public InternalServerException(string message)
            : base(message) { }

        public InternalServerException(Exception ex)
            : base(ex.Message) { }

        public InternalServerException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}