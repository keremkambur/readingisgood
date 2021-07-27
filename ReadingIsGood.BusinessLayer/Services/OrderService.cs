using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReadingIsGood.BusinessLayer.Contracts;
using ReadingIsGood.BusinessLayer.Exceptions;
using ReadingIsGood.BusinessLayer.RequestModels.Order;
using ReadingIsGood.BusinessLayer.ResponseModels.Order;
using ReadingIsGood.BusinessLayer.ResponseModels.Product;
using ReadingIsGood.EntityLayer.Database.Content;

namespace ReadingIsGood.BusinessLayer.Services
{
    public class OrderService : InternalService, IOrderService
    {
        public OrderService(ILogger<OrderService> logger,
            IBusinessObject businessObject) : base(logger, businessObject)
        {
        }

        public IList<OrderListItemResponse> GetOrderList(Guid userUuid, CancellationToken cancellationToken)
        {
            return BusinessObject.DatabaseRepository.OrderCrudOperations.QueryDbSet().Include(i => i.OrderDetails)
                .Where(x => x.User.Uuid == userUuid)
                .Select(x => new OrderListItemResponse
                {
                    Address = x.Address,
                    OrderDate = x.OrderDate,
                    Products = x.OrderDetails.Select(x => new ProductResponse
                    {
                        Name = x.Product.Name,
                        Quantity = x.Quantity,
                        UnitPrice = x.Product.UnitPrice
                    }).ToList()
                }).ToList();
        }

        public Task<OrderDetailResponse> GetOrderDetail(Guid userUuid, Guid orderUuid, CancellationToken cancellationToken)
        {
            var orderWithDetails = this
                .BusinessObject
                .DatabaseRepository
                .OrderCrudOperations
                .QueryDbSet()
                .Include(x => x.User)
                .Include(x => x.OrderDetails)
                .ThenInclude<Order, OrderDetail, Product>(x => x.Product)
                .SingleOrDefault(x => x.Uuid == orderUuid && x.User.Uuid == userUuid);

            if (orderWithDetails == null)
            {
                throw new ForbiddenAccessException("Either this order couldn't found or you don't have permission!");
            }

            return Task.FromResult(new OrderDetailResponse
            {
                Address = orderWithDetails.Address,
                OrderDate = orderWithDetails.OrderDate,
                OrderUuid = orderWithDetails.Uuid,
                PurchasedItems = orderWithDetails.OrderDetails.Select(x => new PurchasedItems
                {
                    ProductId = x.Product.Uuid,
                    Quantity = x.Quantity,
                    UnitPrice = x.Product.UnitPrice
                }).ToList()
            });

        }

        public async Task<IList<ProductQuantityData>> Order(Guid userUuid, OrderRequest request, CancellationToken cancellationToken)
        {
            var groupedProductQuantities = request.ProductQuantities.GroupBy(x => x.ProductUuid).Select(g =>
                new ProductQuantityData
                {
                    ProductUuid = g.Key,
                    Quantity = g.Sum(s => s.Quantity)
                }).ToList();
            
            var productsInOrder = BusinessObject.DatabaseRepository.ProductCrudOperations.QueryList(x =>
                groupedProductQuantities.Select(g => g.ProductUuid).Contains(x.Uuid));

            var filteredInsufficientProducts = groupedProductQuantities
                .Where(x => x.Quantity <= productsInOrder.First(p => p.Uuid == x.ProductUuid).AmountLeft).ToList();


            var createdOrder = BusinessObject.DatabaseRepository.OrderCrudOperations.Create(new Order
            {
                User = BusinessObject.AuthRepository.UserCrudOperations.QuerySingle(x => x.Uuid == userUuid),
                OrderDate = DateTime.Now,
                Address = request.Address,
                OrderStatus = EntityLayer.Enum.OrderStatus.Created
            });

            createdOrder.OrderDetails = productsInOrder.Select(p =>
            {
                var quantity = filteredInsufficientProducts
                                .Where(x => x.ProductUuid == p.Uuid)
                                .Sum(x => x.Quantity);

                return new OrderDetail
                {
                    Product = p,
                    Quantity = quantity,
                    PriceSum = p.UnitPrice * quantity
                };
            }).ToList();


            filteredInsufficientProducts
                .ForEach(x => this.BusinessObject
                                                    .DatabaseRepository
                                                    .ProductCrudOperations
                                                    .Update(x.ProductUuid, 
                                                        product 
                                                                        => product.AmountLeft -= x.Quantity));

            await BusinessObject.DatabaseRepository.CommitChangesAsync();

            return filteredInsufficientProducts;
        }
    }
}
