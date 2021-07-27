using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingIsGood.BusinessLayer.Contracts;
using ReadingIsGood.BusinessLayer.Extensions;
using ReadingIsGood.BusinessLayer.RequestModels.Product;
using ReadingIsGood.BusinessLayer.ResponseModels.Product;
using ReadingIsGood.DataLayer.Contracts;

namespace ReadingIsGood.BusinessLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly IDatabaseRepository _databaseRepository;

        public ProductService(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }

        public IList<ProductResponse> GetAvailableProductList()
        {
            return this._databaseRepository.ProductCrudOperations.QueryList(x => x.StockCount > 0)
                .Select(x => new ProductResponse {Name = x.Name, Stock = x.StockCount}).ToList();
        }

        public Task CreateProductOrIncreaseStock(ProductRequest request)
        {
            var existingProduct = this._databaseRepository.ProductCrudOperations.QueryDbSet().FirstOrDefault(x => string.Equals(x.Name, request.Name, StringComparison.CurrentCultureIgnoreCase));

            if (existingProduct != null)
            {
                this._databaseRepository.ProductCrudOperations.Update(existingProduct.Uuid,
                    product => product.StockCount = product.StockCount + request.Quantity);
            }
            else
            {
                this._databaseRepository.ProductCrudOperations.Create(request.ToProduct());
            }

            return Task.CompletedTask;
        }
    }
}
