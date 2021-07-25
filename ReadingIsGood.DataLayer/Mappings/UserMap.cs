using System;
using Microsoft.EntityFrameworkCore;
using ReadingIsGood.DataLayer.Mappings.Base;
using ReadingIsGood.EntityLayer.Database;
using ReadingIsGood.EntityLayer.Enum;
using ReadingIsGood.Utils.Extensions;

namespace ReadingIsGood.DataLayer.Mappings
{
    public class UserMap : EntityMap<User>
    {
        public UserMap(string schema = "") : base(schema)
        {
        }

        public override void Map(ModelBuilder modelBuilder)
        {
            // get entity builder reference
            var entity = modelBuilder.Entity<User>();

            // set table name and schema
            this.MapTableAndSchema(entity);

            // apply component mappings
            this.EntityMapComponents.ForEach(c => c.Map(entity));

            // identifier
            entity.HasKey(p => p.UserId);
            entity.Property(p => p.UserId).UseIdentityColumn();

            entity.Property(p => p.Email).HasMaxLength(200).IsUnicode(false);
            entity.Property(p => p.FirstName).HasMaxLength(200).IsUnicode(false);
            entity.Property(p => p.LastName).HasMaxLength(200).IsUnicode(false);
            entity.Property(p => p.PasswordHashed).HasMaxLength(256).IsUnicode(false);

            entity
                .Property(p => p.UserType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue(UserType.Customer)
                .HasConversion(
                    v => v.ToString(),
                    v => v.ConvertToEnum(UserType.Unknown, true)
                );

            entity.HasMany(r => r.Orders).WithOne(r => r.User).HasForeignKey(r => r.UserId);
        }
    }
}
