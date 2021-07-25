using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReadingIsGood.DataLayer.Extensions;
using ReadingIsGood.EntityLayer.Database;
using ReadingIsGood.EntityLayer.Database.Base;

namespace ReadingIsGood.DataLayer.Mappings.Base
{
    public abstract class EntityMap<TEntity> : IEntityMap where TEntity : class, IEntity
    {
        protected readonly string Schema;
        protected readonly List<IEntityMapComponent> EntityMapComponents;

        protected EntityMap(string schema)
        {
            this.Schema = schema;
            this.EntityMapComponents = new List<IEntityMapComponent>();

            if (typeof(TEntity).HasInterface<IEntity>())
            {
                this.EntityMapComponents.Add(new EntityMapComponent());
            }
        }

        public abstract void Map(ModelBuilder modelBuilder);

        protected void MapTableAndSchema(EntityTypeBuilder<TEntity> entity)
        {
            entity.ToTable(
                typeof(TEntity).Name,
                string.IsNullOrEmpty(this.Schema) ? typeof(TEntity).LastNamespacePart() : this.Schema);
        }
    }
}