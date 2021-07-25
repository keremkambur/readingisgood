using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingIsGood.EntityLayer.Database.Base;
using ReadingIsGood.EntityLayer.Enum;

namespace ReadingIsGood.EntityLayer.Database
{
    public class Order : IAuditEntity
    {
        #region IAuditEntity

        public int Id => OrderId;
        public Guid Uuid { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedBy { get; set; } 
        #endregion

        public int OrderId { get; set; }

        public uint Quantity { get; set; }

        public DateTime OrderDate { get; set; }

        public string Address { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
