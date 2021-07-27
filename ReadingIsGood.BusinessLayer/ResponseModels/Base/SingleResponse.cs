namespace ReadingIsGood.BusinessLayer.ResponseModels.Base
{
    public class SingleResponse<TModel> : Response, ISingleResponse<TModel> where TModel : new()
    {
        public SingleResponse(string requestId)
        {
            this.Model = new TModel();
            this.Status = 200;
            this.RequestId = requestId;
        }

        /// <inheritdoc />
        public TModel Model { get; set; }
    }
}
