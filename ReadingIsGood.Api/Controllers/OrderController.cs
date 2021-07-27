using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReadingIsGood.BusinessLayer.Contracts;
using ReadingIsGood.EntityLayer.Database.Content;

namespace ReadingIsGood.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private static readonly string[] Summaries =
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IBusinessObject _businessObject;

        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger, IBusinessObject businessObject)
        {
            _logger = logger;
            _businessObject = businessObject;
        }

        [HttpGet]
        [Authorize]
        public void GetList()
        {
            
        }
    }
}