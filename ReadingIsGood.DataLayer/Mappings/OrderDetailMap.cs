using Microsoft.EntityFrameworkCore;
using ReadingIsGood.DataLayer.Mappings.Base;
using ReadingIsGood.EntityLayer.Database.Content;
using ReadingIsGood.EntityLayer.Enum;
using ReadingIsGood.Utils.Extensions;

namespace ReadingIsGood.DataLayer.Mappings
{
    public class OrderDetailMap : EntityMap<OrderDetail>
    {
        public OrderDetailMap(string schema = "") : base(schema)
        {
        }

        public override void Map(ModelBuilder modelBuilder)
        {
            // get entity builder reference
            var entity = modelBuilder.Entity<OrderDetail>();

            // set table name and schema
            MapTableAndSchema(entity);

            // apply component mappings
            EntityMapComponents.ForEach(c => c.Map(entity));

            // identifier
            entity.HasKey(p => p.OrderDetailId);
            entity.Property(p => p.OrderDetailId).UseIdentityColumn();

            entity.HasOne(r => r.Order).WithMany(x => x.OrderDetails);
            entity.HasOne(x => x.Product).WithMany(x => x.OrderDetails);
        }
    }
}