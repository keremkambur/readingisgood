using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReadingIsGood.BusinessLayer.Contracts;
using ReadingIsGood.BusinessLayer.Extensions;
using ReadingIsGood.BusinessLayer.RequestModels.Product;
using ReadingIsGood.BusinessLayer.ResponseModels.Product;
using ReadingIsGood.DataLayer.Contracts;

namespace ReadingIsGood.BusinessLayer.Services
{
    public class ProductService : InternalService, IProductService
    {
        public ProductService(ILogger<ProductService> logger,
            IBusinessObject businessObject) : base(logger, businessObject)
        {
        }

        public IList<ProductResponse> GetAvailableProductList()
        {
            return BusinessObject.DatabaseRepository.ProductCrudOperations.QueryList(x => x.AmountLeft > 0)
                .Select(x => new ProductResponse {Name = x.Name, Quantity = x.AmountLeft, UnitPrice = x.UnitPrice, ProductUuid = x.Uuid}).ToList();
        }
        
        public Task CreateProductOrIncreaseStock()
        {
            BusinessObject.EnableBulkMode();

            foreach (var i in Enumerable.Range(0, 1000))
            {
                var rndQuantity = new Random().Next(1, 1000);
                var rndPrice = new Random().Next(1, 1000);
                var rndName = $"test-{rndQuantity}";

                var existingProduct = BusinessObject.DatabaseRepository.ProductCrudOperations.QuerySingle(x => string.Equals(x.Name, rndName, StringComparison.CurrentCultureIgnoreCase));

                if (existingProduct != null)
                {
                    BusinessObject.DatabaseRepository.ProductCrudOperations.Update(existingProduct.Uuid,
                        product => product.AmountLeft = product.AmountLeft + rndQuantity);
                }
                else
                {
                    BusinessObject.DatabaseRepository.ProductCrudOperations.Create(new ProductRequest{Name = rndName, Quantity = rndQuantity, Price = rndPrice }.ToProduct());
                }

            }

            BusinessObject.DatabaseRepository.DisableBulkMode(true);

            return Task.CompletedTask;
        }
    }
}
