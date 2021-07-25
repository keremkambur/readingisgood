using System;
using Microsoft.EntityFrameworkCore;
using ReadingIsGood.DataLayer.Mappings.Base;
using ReadingIsGood.EntityLayer.Database;
using ReadingIsGood.EntityLayer.Enum;
using ReadingIsGood.Utils.Extensions;

namespace ReadingIsGood.DataLayer.Mappings
{
    public class OrderMap : EntityMap<Order>
    {
        public OrderMap(string schema = "") : base(schema)
        {
        }

        public override void Map(ModelBuilder modelBuilder)
        {
            // get entity builder reference
            var entity = modelBuilder.Entity<Order>();

            // set table name and schema
            this.MapTableAndSchema(entity);

            // apply component mappings
            this.EntityMapComponents.ForEach(c => c.Map(entity));

            // identifier
            entity.HasKey(p => p.OrderId);
            entity.Property(p => p.OrderId).UseIdentityColumn();

            entity.Property(p => p.Address).HasMaxLength(500).IsUnicode(false);

            entity
                .Property(p => p.OrderStatus)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue(OrderStatus.Created)
                .HasConversion(
                    v => v.ToString(),
                    v => v.ConvertToEnum(OrderStatus.Unknown, true)
                );

            //entity.OwnsMany(r => r.Products).WithOwner()
        }
    }
}
