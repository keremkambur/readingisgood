using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ReadingIsGood.DataLayer.Mappings.Base
{
    public interface IEntityMapComponent
    {
        void Map(EntityTypeBuilder entity);
    }
}