using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReadingIsGood.BusinessLayer.ResponseModels.Order
{
    public class PurchasedItems
    {
        [JsonPropertyName("productId")]
        public Guid ProductId { get; set; }

        [JsonPropertyName("unitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
}
