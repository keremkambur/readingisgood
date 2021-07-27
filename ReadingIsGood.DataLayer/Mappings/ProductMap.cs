using Microsoft.EntityFrameworkCore;
using ReadingIsGood.DataLayer.Mappings.Base;
using ReadingIsGood.EntityLayer.Database.Content;

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
            MapTableAndSchema(entity);

            // apply component mappings
            EntityMapComponents.ForEach(c => c.Map(entity));

            // identifier
            entity.HasKey(p => p.ProductId);
            entity.Property(p => p.ProductId).UseIdentityColumn();
            
            entity.HasMany(r => r.Orders).WithMany(x => x.Products);
        }
    }
}