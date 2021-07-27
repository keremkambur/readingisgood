using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ReadingIsGood.BusinessLayer.Extensions;
using ReadingIsGood.BusinessLayer.RequestModels.Product;
using ReadingIsGood.BusinessLayer.ResponseModels.Base;
using ReadingIsGood.BusinessLayer.ResponseModels.Order;

namespace ReadingIsGood.Api.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet]
        [Authorize]
        public ListResponse<OrderListItemResponse> GetList()
        {
            //user_uuid = HttpContext.User.FindFirst("http://reading-is-good.getir.de/api/2021/07/auth/claims/client-id").Value
            var user = HttpContext.User;
            // return all the orders belong to that user
        }
    }
}
