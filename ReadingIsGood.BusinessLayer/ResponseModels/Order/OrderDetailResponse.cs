﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.BusinessLayer.ResponseModels.Order
{
    public class OrderDetailResponse
    {
        public Guid OrderUuid { get; set; }

        public DateTime OrderDate { get; set; }

        //public IEnumerable<ProductRespons> Type { get; set; }
    }
}
