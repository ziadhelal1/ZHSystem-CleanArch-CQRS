using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHSystem.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } = null!;
        
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        
    }
}
