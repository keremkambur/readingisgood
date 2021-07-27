using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ReadingIsGood.DataLayer.Mappings.Base
{
    public class EntityMapper : IEntityMapper
    {
        public EntityMapper()
        {
            Mappings = new List<IEntityMap>
            {
                new UserMap(),
                new OrderMap(),
                new ProductMap(),
                new RefreshTokenMap(),
                new OrderDetailMap()
            };
        }

        public List<IEntityMap> Mappings { get; }

        public void MapEntities(ModelBuilder modelBuilder)
        {
            Mappings.ForEach(item => item.Map(modelBuilder));
        }
    }
}