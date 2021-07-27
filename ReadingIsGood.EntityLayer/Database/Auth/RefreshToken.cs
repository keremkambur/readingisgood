using System;
using ReadingIsGood.EntityLayer.Database.Base;

namespace ReadingIsGood.EntityLayer.Database.Auth
{
    public class RefreshToken : IAuditEntity
    {
        // system
        public int RefreshTokenId { get; set; }

        // domain
        public Guid ClientId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool Rejected { get; set; }

        public User User { get; set; }

        // virtual | IEntity
        public int Id => RefreshTokenId;
        public Guid Uuid { get; set; }

        // IAuditEntity
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}