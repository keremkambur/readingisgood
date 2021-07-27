namespace ReadingIsGood.BusinessLayer.RequestModels.Base
{
    public interface IRequest
    {
        bool ValidateModel();
        void ValidateAndThrow();
    }
}