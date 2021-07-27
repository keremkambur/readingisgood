using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.BusinessLayer.RequestModels.Product
{
    public class ProductRequest
    {
        /// <summary>
        /// If there is a product with the same name, (case insensitive) number will be increased as much as the Quantity passed.
        /// </summary>
        public string Name { get; set; }

        public int Quantity { get; set; } = 1;
    }
}
