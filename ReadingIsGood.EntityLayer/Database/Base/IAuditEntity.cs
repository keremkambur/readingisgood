using System;

namespace ReadingIsGood.EntityLayer.Database.Base
{
    public interface IAuditEntity : IEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime? ModifiedAt { get; set; }
    }
}