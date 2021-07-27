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
        IList<OrderListItemResponse> GetOrderList(Guid userUuid, CancellationToken cancellationToken);

        Task<OrderDetailResponse> GetOrderDetail(Guid userUuid, Guid orderUuid, CancellationToken cancellationToken);

        Task<IList<ProductQuantityData>> Order(Guid userUuid, OrderRequest request, CancellationToken cancellationToken);
    }
}
