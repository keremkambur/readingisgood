using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReadingIsGood.BusinessLayer.RequestModels.Product
{
    public class ProductRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; } = 1;

        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }
}
