using System.Collections.Generic;

namespace ReadingIsGood.BusinessLayer.ResponseModels.Base
{
    public class ListResponse<TModel> : Response, IListResponse<TModel>
    {
        public ListResponse(string requestId)
        {
            this.Status = 200;
            this.RequestId = requestId;
            this.Model = new List<TModel>();
        }

        /// <inheritdoc />
        public IList<TModel> Model { get; set; }
    }
}
