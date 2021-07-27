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

        public IList<OrderListItemResponse> GetOrderList(Guid userUuid, CancellationToken cancellationToken)
        {
            return _databaseRepository.OrderCrudOperations.QueryDbSet().Include(i => i.OrderDetails)
                .Where(x => x.User.Uuid == userUuid)
                .Select(x => new OrderListItemResponse
                {
                    Address = x.Address,
                    OrderDate = x.OrderDate,
                    Products = x.OrderDetails.Select(x => new ProductResponse
                    {
                        Name = x.Product.Name,
                        Quantity = x.Quantity
                    }).ToList()
                }).ToList();
        }

        public Task<OrderDetailResponse> GetOrderDetail(Guid userUuid, string orderUuid, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Order(Guid userUuid, OrderRequest request, CancellationToken cancellationToken)
        {
            var productsInOrder = _databaseRepository.ProductCrudOperations.QueryList(x =>
                request.ProductQuantities.Select(x => x.ProductUuid).Contains(x.Uuid));


            var createdOrder = _databaseRepository.OrderCrudOperations.Create(new Order
            {
                User = _authRepository.UserCrudOperations.QuerySingle(x => x.Uuid == userUuid),
                OrderDate = DateTime.Now,
                Address = request.Address
            });

            createdOrder.OrderDetails = productsInOrder.Select(p =>
            {
                var quantity = request
                                .ProductQuantities
                                .Where(x => x.ProductUuid == p.Uuid)
                                .Sum(x => x.Quantity);

                return new OrderDetail
                {
                    Product = p,
                    Quantity = quantity,
                    Price = p.UnitPrice * quantity
                };
            }).ToList();

            return Task.CompletedTask;
        }
    }
}
