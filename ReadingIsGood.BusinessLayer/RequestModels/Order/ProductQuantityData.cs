using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReadingIsGood.BusinessLayer.RequestModels.Order
{
    public class ProductQuantityData
    {
        [JsonPropertyName("productUuid")]
        public Guid ProductUuid { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
}
