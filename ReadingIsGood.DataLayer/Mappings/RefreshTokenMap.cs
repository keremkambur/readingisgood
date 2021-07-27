using System;
using Microsoft.EntityFrameworkCore;
using ReadingIsGood.DataLayer.Extensions;
using ReadingIsGood.DataLayer.Mappings.Base;
using ReadingIsGood.EntityLayer.Database.Auth;

namespace ReadingIsGood.DataLayer.Mappings
{
    public class RefreshTokenMap : EntityMap<RefreshToken>
    {
        public RefreshTokenMap(string schema = "")
            : base(schema)
        {
        }

        public override void Map(ModelBuilder modelBuilder)
        {
            // get entity builder reference
            var entity = modelBuilder.Entity<RefreshToken>();

            // set table name and schema
            MapTableAndSchema(entity);

            // apply component mappings
            EntityMapComponents.ForEach(c => c.Map(entity));

            // identifier
            entity.HasKey(p => p.RefreshTokenId);
            entity.Property(p => p.RefreshTokenId).UseIdentityColumn();

            // domain
            entity.Property(p => p.ClientId).HasDefaultValue(Guid.Empty);
            entity.Property(p => p.Token).HasMaxLength(150).IsUnicode(false);
            entity.Property(p => p.ExpiresAt).IsDatetime2();
            entity.Property(p => p.Rejected).HasDefaultValue(false);
        }
    }
}