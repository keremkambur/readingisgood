using System.Collections.Generic;
using System.Threading.Tasks;
using ReadingIsGood.BusinessLayer.ResponseModels.Product;

namespace ReadingIsGood.BusinessLayer.Contracts
{
    public interface IProductService
    {
        public IList<ProductResponse> GetAvailableProductList();

        Task CreateProductOrIncreaseStock();
    }
}
