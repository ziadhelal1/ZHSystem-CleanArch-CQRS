using ZHSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHSystem.Application.Common.Interfaces
{
    public interface IAuditService
    {
        Task WriteAsync(AuditLog log, CancellationToken cancellationToken = default);
    }
}
