using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadingIsGood.DataLayer.Extensions;
using ReadingIsGood.EntityLayer.Database.Base;

namespace ReadingIsGood.DataLayer.Mappings.Base
{
    public abstract class EntityMap<TEntity> : IEntityMap where TEntity : class, IEntity
    {
        protected readonly List<IEntityMapComponent> EntityMapComponents;
        protected readonly string Schema;

        protected EntityMap(string schema)
        {
            Schema = schema;
            EntityMapComponents = new List<IEntityMapComponent>();

            if (typeof(TEntity).HasInterface<IEntity>()) EntityMapComponents.Add(new EntityMapComponent());
        }

        public abstract void Map(ModelBuilder modelBuilder);

        protected void MapTableAndSchema(EntityTypeBuilder<TEntity> entity)
        {
            entity.ToTable(
                typeof(TEntity).Name,
                string.IsNullOrEmpty(Schema) ? typeof(TEntity).LastNamespacePart() : Schema);
        }
    }
}