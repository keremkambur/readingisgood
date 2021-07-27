using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingIsGood.BusinessLayer.RequestModels.Order;

namespace ReadingIsGood.BusinessLayer.ResponseModels.Order
{
    public class OrderDetailResponse
    {
        public Guid OrderUuid { get; set; }

        public DateTime OrderDate { get; set; }

        public string Address { get; set; }

        public IList<PurchasedItems> PurchasedItems { get; set; }

        //public IEnumerable<ProductRespons> Type { get; set; }
    }
}
