using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadingIsGood.BusinessLayer.Contracts;
using ReadingIsGood.BusinessLayer.Extensions;
using ReadingIsGood.BusinessLayer.RequestModels.Order;
using ReadingIsGood.BusinessLayer.ResponseModels.Base;
using ReadingIsGood.BusinessLayer.ResponseModels.Order;
using ReadingIsGood.Utils.Identity;

namespace ReadingIsGood.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ListResponse<OrderListItemResponse>> GetList(CancellationToken cancellationToken)
        {
            var userUuid = HttpContext.User.FindFirst(ClaimsIdentityHelper.ClaimsTypes.ClientId)?.Value;
            var response = new ListResponse<OrderListItemResponse>(this.HttpContext.TraceIdentifier);
            
            try
            {
                response.Model = await this._orderService.GetOrderList(userUuid, cancellationToken);
            }
            catch (Exception e)
            {
                response.SetError(e);
                response.Model = null;
            }

            return response;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<SingleResponse<OrderDetailResponse>> GetSpecific(string orderUuid, CancellationToken cancellationToken)
        {
            var userUuid = HttpContext.User.FindFirst(ClaimsIdentityHelper.ClaimsTypes.ClientId)?.Value;
            var response = new SingleResponse<OrderDetailResponse>(this.HttpContext.TraceIdentifier);

            try
            {
                response.Model = await this._orderService.GetOrderDetail(userUuid, orderUuid, cancellationToken);
            }
            catch (Exception e)
            {
                response.SetError(e);
                response.Model = null;
            }

            return response;
        }

        [HttpPost]
        [Authorize]
        public async Task<PostResponse> Order(OrderRequest request, CancellationToken cancellationToken)
        {
            var response = new PostResponse(this.HttpContext.TraceIdentifier);

            try
            {
                request.ValidateAndThrow();

                await _orderService.Order(request, cancellationToken)
                        .ConfigureAwait(false)
                    ;
            }
            catch (Exception ex)
            {
                response.SetError(ex);
            }

            return response;
        }
    }
}