using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingIsGood.BusinessLayer.ResponseModels.Base;
using ReadingIsGood.BusinessLayer.ResponseModels.Order;

namespace ReadingIsGood.BusinessLayer.Contracts
{
    public interface IOrderService
    {
        Task<ListResponse<OrderDetailResponse>> GetOrderList(string userUuid);

        Task<SingleResponse<OrderDetailResponse>> GetOrderDetail(string userUuid, string orderUuid);

        Task<PostResponse> Order(string userUuid, string productUuid);
    }
}
