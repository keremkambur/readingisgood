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
using ReadingIsGood.EntityLayer.Database.Content;

namespace ReadingIsGood.BusinessLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IDatabaseRepository _databaseRepository;
        private readonly IAuthRepository _authRepository;

        public OrderService(IDatabaseRepository databaseRepository, IAuthRepository authRepository)
        {
            _databaseRepository = databaseRepository;
            _authRepository = authRepository;
        }

        public Task<IList<OrderListItemResponse>> GetOrderList(string userUuid, CancellationToken cancellationToken)
        {
            return _databaseRepository.OrderCrudOperations.QueryDbSet().Include(i => i.Products)
                .Where(x => x.User.Uuid.ToString() == userUuid)
                .Select(x => new OrderListItemResponse
                {
                    Address = x.Address,
                    OrderDate = x.OrderDate,
                    Products = x.Products.Select(p => new ProductResponse {Name = p.Name, Quantity = x.Products.})
                });
        }

        public Task<OrderDetailResponse> GetOrderDetail(string userUuid, string orderUuid, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PostResponse> Order(OrderRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PostResponse> Order(string userUuid, OrderRequest request, CancellationToken cancellationToken)
        {
            _databaseRepository.OrderCrudOperations.Create(new Order
            {
                User = _authRepository.UserCrudOperations.QuerySingle(x => x.Uuid == Guid.Parse(userUuid)),
                OrderDate = DateTime.Now,
                Products = request.

            })
        }
    }
}
