using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingIsGood.BusinessLayer.RequestModels.Product;
using ReadingIsGood.BusinessLayer.ResponseModels.Base;
using ReadingIsGood.BusinessLayer.ResponseModels.Product;

namespace ReadingIsGood.BusinessLayer.Contracts
{
    public interface IProductService
    {
        IList<ProductResponse> GetAvailableProductList();

        Task CreateProductOrIncreaseStock(ProductRequest request);
    }
}
