using System;
using System.Collections.Generic;
using ReadingIsGood.EntityLayer.Database.Base;

namespace ReadingIsGood.EntityLayer.Database.Content
{
    public class Product : IAuditEntity
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public int AmountLeft { get; set; }

        public decimal UnitPrice { get; set; }

        #region IAuditEntity

        public int Id => ProductId;
        public Guid Uuid { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        #endregion
    }
}