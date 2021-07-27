using ReadingIsGood.EntityLayer.Enum;

namespace ReadingIsGood.BusinessLayer.ResponseModels.Base
{
    public interface IErrorResponse
    {
        ErrorCodes Code { get; set; }
        string Message { get; set; }
        object Payload { get; set; }
    }
}