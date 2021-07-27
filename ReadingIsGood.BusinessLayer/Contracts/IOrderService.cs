using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReadingIsGood.BusinessLayer.RequestModels.Order;
using ReadingIsGood.BusinessLayer.ResponseModels.Base;
using ReadingIsGood.BusinessLayer.ResponseModels.Order;

namespace ReadingIsGood.BusinessLayer.Contracts
{
    public interface IOrderService
    {
        Task<IList<OrderListItemResponse>> GetOrderList(string userUuid, CancellationToken cancellationToken);

        Task<OrderDetailResponse> GetOrderDetail(string userUuid, string orderUuid, CancellationToken cancellationToken);

        Task<PostResponse> Order(OrderRequest request, CancellationToken cancellationToken);
    }
}
