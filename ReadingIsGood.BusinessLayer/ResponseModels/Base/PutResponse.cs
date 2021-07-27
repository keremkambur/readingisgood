namespace ReadingIsGood.BusinessLayer.ResponseModels.Base
{
    public class PutResponse : Response
    {
        public PutResponse(string requestId)
        {
            this.Status = (int)System.Net.HttpStatusCode.OK;
            this.RequestId = requestId;
        }
    }

    public class PutResponse<TModel> : PutResponse
    {
        public TModel Model { get; set; }

        public PutResponse(string requestId) : base(requestId)
        {
        }
    }
}
