using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingIsGood.EntityLayer.Database.Base;
using ReadingIsGood.EntityLayer.Enum;

namespace ReadingIsGood.EntityLayer.Database
{
    public class User : IAuditEntity
    {
        #region IAuditEntity

        public int Id => this.UserId;
        public Guid Uuid { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        #endregion

        public int UserId { get; set; }

        public UserType UserType { get; set; }

        public string Email { get; set; }

        public string PasswordHashed { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
