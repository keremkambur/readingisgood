namespace ReadingIsGood.BusinessLayer.RequestModels.Base
{
    public abstract class Request : IRequest
    {
        public virtual bool ValidateModel()
        {
            try
            {
                ValidateAndThrow();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public abstract void ValidateAndThrow();
    }
}