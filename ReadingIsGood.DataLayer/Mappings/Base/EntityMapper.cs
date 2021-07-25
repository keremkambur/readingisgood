using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ReadingIsGood.DataLayer.Mappings.Base
{
    public class EntityMapper : IEntityMapper
    {
        public EntityMapper()
        {
            this.Mappings = new List<IEntityMap>
            {
                new OrderMap() as IEntityMap,
                new UserMap() as IEntityMap,
                new ProductMap() as IEntityMap,
            };
        }

        public List<IEntityMap> Mappings { get; }

        public void MapEntities(ModelBuilder modelBuilder) 
            => this.Mappings.ForEach(item => item.Map(modelBuilder));
    }
}