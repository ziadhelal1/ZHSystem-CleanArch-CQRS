using ZHSystem.Application.Common;
using ZHSystem.Application.Common.Interfaces;
using ZHSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHSystem.Infrastructure.Services
{
    public class AuditService:IAuditService
    {
        private readonly IApplicationDbContext _db;

        public AuditService(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task WriteAsync(AuditLog log, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Audit Log Triggred ");
            await _db.AuditLogs.AddAsync(log, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
