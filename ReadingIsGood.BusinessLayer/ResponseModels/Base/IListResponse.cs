using System.Collections.Generic;

namespace ReadingIsGood.BusinessLayer.ResponseModels.Base
{
    public interface IListResponse<TModel> : IResponse
    {
        /// <summary>
        /// List of models.
        /// </summary>
        IList<TModel> Model { get; set; }
    }
}
