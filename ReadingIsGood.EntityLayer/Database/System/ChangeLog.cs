using System;
using ReadingIsGood.EntityLayer.Database.Base;

namespace ReadingIsGood.EntityLayer.Database.System
{
    public class ChangeLog : IEntity
    {
        // system
        public int ChangeLogId { get; set; }

        // domain
        public DateTime CreatedAt { get; set; }
        public string Application { get; set; }
        public int ObjectId { get; set; }
        public Guid ObjectUuid { get; set; }
        public string ClassName { get; set; }
        public string OriginalValue { get; set; }
        public string CurrentValue { get; set; }
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        public string State { get; set; }
        public Guid BatchUuid { get; set; }
        public int Id => ChangeLogId;
        public Guid Uuid { get; set; }
    }
}