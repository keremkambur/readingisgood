using System;
using System.Collections.Generic;
using ReadingIsGood.EntityLayer.Database.Base;

namespace ReadingIsGood.EntityLayer.Database
{
    public class Product : IAuditEntity
    {
        #region IAuditEntity

        public int Id => ProductId;
        public Guid Uuid { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedBy { get; set; } 

        #endregion

        public int ProductId { get; set; }
        
        public string Name { get; set; }

        public int Count { get; set; }
    }
}