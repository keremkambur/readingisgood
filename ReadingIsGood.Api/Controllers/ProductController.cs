using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ReadingIsGood.BusinessLayer.Contracts;
using ReadingIsGood.BusinessLayer.Extensions;
using ReadingIsGood.BusinessLayer.RequestModels.Product;
using ReadingIsGood.BusinessLayer.ResponseModels.Base;
using ReadingIsGood.BusinessLayer.ResponseModels.Order;
using ReadingIsGood.BusinessLayer.ResponseModels.Product;

namespace ReadingIsGood.Api.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Authorize]
        public Task<ListResponse<ProductResponse>> GetList()
        {
            var response = new ListResponse<ProductResponse>(HttpContext.TraceIdentifier);
            
            try
            {
                response.Model = _productService.GetAvailableProductList();
            }
            catch (Exception e)
            {
                response.SetError(e);
                response.Model = null;
            }

            return Task.FromResult(response);
        }

        [HttpPost("generate-products")]
        [Authorize]
        public async Task<PostResponse> GenerateProducts()
        {
            var response = new PostResponse(HttpContext.TraceIdentifier);

            try
            {
                await _productService.CreateProductOrIncreaseStock();
            }
            catch (Exception e)
            {
                response.SetError(e);
            }

            return response;
        }
    }
}
