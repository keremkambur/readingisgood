using System;

namespace ReadingIsGood.EntityLayer.QueryModels
{
    public class QueryUserFromLoginResponse
    {
        public Guid ClientId { get; set; }
        public Guid Uuid { get; set; }
    }
}