using System;
using Microsoft.EntityFrameworkCore;
using ReadingIsGood.DataLayer.Mappings.Base;
using ReadingIsGood.EntityLayer.Database;

namespace ReadingIsGood.DataLayer.Mappings
{
    public class ProductMap : EntityMap<Product>
    {
        public ProductMap(string schema = "") : base(schema)
        {
        }

        public override void Map(ModelBuilder modelBuilder)
        {
            // get entity builder reference
            var entity = modelBuilder.Entity<Product>();

            // set table name and schema
            this.MapTableAndSchema(entity);

            // apply component mappings
            this.EntityMapComponents.ForEach(c => c.Map(entity));

            // identifier
            //entity.HasKey(p => p.OrderId);
            //entity.Property(p => p.OrderId).UseIdentityColumn();
        }
    }
}
