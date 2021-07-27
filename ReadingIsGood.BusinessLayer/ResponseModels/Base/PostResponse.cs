

namespace ReadingIsGood.BusinessLayer.ResponseModels.Base
{
    public class PostResponse : Response
    {
        public PostResponse(string requestId)
        {
            this.Status = (int)System.Net.HttpStatusCode.OK;
            this.RequestId = requestId;
        }
    }

    public class PostResponse<TModel> : PostResponse
    {
        public TModel Model { get; set; }

        public PostResponse(string requestId) : base(requestId)
        {
        }
    }
}
