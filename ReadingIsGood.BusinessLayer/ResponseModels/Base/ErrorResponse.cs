using ReadingIsGood.EntityLayer.Enum;

namespace ReadingIsGood.BusinessLayer.ResponseModels.Base
{
    public class ErrorResponse : IErrorResponse
    {
        public ErrorCodes Code { get; set; }
        public string Message { get; set; }
        public object Payload { get; set; }
    }
}
