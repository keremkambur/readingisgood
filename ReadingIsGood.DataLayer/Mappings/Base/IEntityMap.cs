using Microsoft.EntityFrameworkCore;

namespace ReadingIsGood.DataLayer.Mappings.Base
{
    public interface IEntityMap
    {
        void Map(ModelBuilder modelBuilder);
    }
}