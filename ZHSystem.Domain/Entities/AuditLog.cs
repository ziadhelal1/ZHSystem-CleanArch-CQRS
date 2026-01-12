using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHSystem.Domain.Entities
{
    public class AuditLog :BaseEntity
    {
        public string? UserId { get; set; }
        public string? UserEmail { get; set; }

        public string Method { get; set; } = null!;
        public string Path { get; set; } = null!;

        public int StatusCode { get; set; }
        public long DurationMs { get; set; }

        public string? IpAddress { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
