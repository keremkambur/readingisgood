using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingIsGood.BusinessLayer.ResponseModels.Product;

namespace ReadingIsGood.BusinessLayer.ResponseModels.Order
{
    public class OrderListItemResponse
    {
        public IList<ProductResponse> Products { get; set; }

        public string Address { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
