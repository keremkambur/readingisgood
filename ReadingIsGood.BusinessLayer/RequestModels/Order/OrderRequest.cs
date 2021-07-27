using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ReadingIsGood.BusinessLayer.RequestModels.Base;

namespace ReadingIsGood.BusinessLayer.RequestModels.Order
{
    public class OrderRequest : Request
    {
        [JsonPropertyName("productQuantities")]
        public IEnumerable<ProductQuantityData> ProductQuantities { get; set; }
        
        [JsonPropertyName("address")]
        public string Address { get; set; }

        public override void ValidateAndThrow()
        {
            throw new NotImplementedException();
        }
    }
}