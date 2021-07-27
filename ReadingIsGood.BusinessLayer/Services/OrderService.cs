using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReadingIsGood.BusinessLayer.Contracts;
using ReadingIsGood.BusinessLayer.RequestModels.Order;
using ReadingIsGood.BusinessLayer.ResponseModels.Base;
using ReadingIsGood.BusinessLayer.ResponseModels.Order;

namespace ReadingIsGood.BusinessLayer.Services
{
    public class OrderService : IOrderService
    {
        public Task<ListResponse<OrderListItemResponse>> GetOrderList(string userUuid, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderDetailResponse> GetOrderDetail(string userUuid, string orderUuid, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PostResponse> Order(OrderRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
