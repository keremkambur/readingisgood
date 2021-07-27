using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReadingIsGood.BusinessLayer.Contracts;
using ReadingIsGood.BusinessLayer.RequestModels.Order;
using ReadingIsGood.BusinessLayer.ResponseModels.Base;
using ReadingIsGood.BusinessLayer.ResponseModels.Order;
using ReadingIsGood.BusinessLayer.ResponseModels.Product;
using ReadingIsGood.DataLayer.Contracts;

namespace ReadingIsGood.BusinessLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IDatabaseRepository _databaseRepository;

        public OrderService(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }

        public Task<IList<OrderListItemResponse>> GetOrderList(string userUuid, CancellationToken cancellationToken)
        {
            return _databaseRepository.OrderCrudOperations.QueryDbSet().Include(x => x.Products)
                .Where(x => x.User.Uuid.ToString() == userUuid)
                .Select(x => new OrderListItemResponse
                {
                    Address = x.Address,
                    OrderDate = x.OrderDate,
                    Products = x.Products.Select(p => new ProductResponse {Name = p.Name, Quantity = })
                });
        }

        public Task<OrderDetailResponse> GetOrderDetail(string userUuid, string orderUuid, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PostResponse> Order(OrderRequest request, CancellationToken cancellationToken)
        {
            
        }
    }
}
