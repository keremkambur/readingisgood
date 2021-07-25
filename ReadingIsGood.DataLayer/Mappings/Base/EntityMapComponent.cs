using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadingIsGood.DataLayer.Extensions;
using ReadingIsGood.EntityLayer.Database.Base;

namespace ReadingIsGood.DataLayer.Mappings.Base
{
    public class EntityMapComponent : IEntityMapComponent
    {
        public void Map(EntityTypeBuilder entity)
        {
            // ignore
            entity.Ignore(nameof(IEntity.Id));

            // identifier
            entity.HasIndex(nameof(IEntity.Uuid)).IsUnique();
            entity.Property(nameof(IEntity.Uuid)).HasDefaultNewId();
        }
    }
}
