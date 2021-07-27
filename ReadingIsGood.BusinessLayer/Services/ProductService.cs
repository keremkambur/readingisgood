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
            return this._databaseRepository.ProductCrudOperations.QueryList(x => x.AmountLeft > 0)
                .Select(x => new ProductResponse {Name = x.Name, Quantity = x.AmountLeft}).ToList();
        }
        
        public Task CreateProductOrIncreaseStock()
        {
            this._databaseRepository.EnableBulkMode();

            foreach (var i in Enumerable.Range(0, 1000))
            {
                var rndNum = new Random().Next(1, 1000);
                var rndName = $"test-{rndNum}";

                var existingProduct = this._databaseRepository.ProductCrudOperations.QueryDbSet().FirstOrDefault(x => string.Equals(x.Name, rndName, StringComparison.CurrentCultureIgnoreCase));

                if (existingProduct != null)
                {
                    this._databaseRepository.ProductCrudOperations.Update(existingProduct.Uuid,
                        product => product.AmountLeft = product.AmountLeft + rndNum);
                }
                else
                {
                    this._databaseRepository.ProductCrudOperations.Create(new ProductRequest{Name = rndName, Quantity = rndNum}.ToProduct());
                }

            }

            this._databaseRepository.DisableBulkMode(true);

            return Task.CompletedTask;
        }
    }
}
