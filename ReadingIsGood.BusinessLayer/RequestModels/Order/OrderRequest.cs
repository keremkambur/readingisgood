﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            if (string.IsNullOrWhiteSpace(Address))
            {
                throw new ArgumentNullException($"Please provide address information");
            }

            if (ProductQuantities == null || !ProductQuantities.Any())
            {
                throw new ArgumentNullException($"Please provide Product information.");
            }
        }
    }
}