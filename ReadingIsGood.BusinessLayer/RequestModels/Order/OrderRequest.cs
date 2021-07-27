using System;
using System.Text.Json.Serialization;
using ReadingIsGood.BusinessLayer.RequestModels.Base;

namespace ReadingIsGood.BusinessLayer.RequestModels.Order
{
    public class OrderRequest : Request
    {
        [JsonPropertyName("productUuid")] public Guid ProductUuid { get; set; }

        [JsonPropertyName("address")] public string Address { get; set; }

        public override void ValidateAndThrow()
        {
            throw new NotImplementedException();
        }
    }
}