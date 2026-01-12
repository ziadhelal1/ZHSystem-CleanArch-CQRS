using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHSystem.Domain.Entities
{
    public class User:BaseEntity 
    {
        
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!; 
        public bool isActive { get; set; }=true;
        public bool EmailVerified { get; set; }
        public string? EmailVerificationTokenHash { get; set; }
        public DateTime? EmailVerificationExpires { get; set; }
        public string? PasswordResetTokenHash { get; set; }
        public DateTime? PasswordResetExpires { get; set; }
        public DateTime? LastSecurityEmailSentAt { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();


    }
}
