using System;

namespace ReadingIsGood.EntityLayer.Database.Base
{
    public interface IEntity
    {
        int Id { get; }
        Guid Uuid { get; }
    }
}