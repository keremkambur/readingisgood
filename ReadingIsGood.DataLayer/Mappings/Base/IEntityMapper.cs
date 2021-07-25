using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ReadingIsGood.DataLayer.Mappings.Base
{
    public interface IEntityMapper
    {
        List<IEntityMap> Mappings { get; }

        void MapEntities(ModelBuilder modelBuilder);
    }
}