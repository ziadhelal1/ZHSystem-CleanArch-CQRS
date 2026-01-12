using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZHSystem.Application.Common;

namespace ZHSystem.Application.Features.Users.Commands
{
    public record RemoveRoleCommand(int UserId, int RoleId) : IRequest;
    public class RemoveRoleCommandHandler
        : IRequestHandler<RemoveRoleCommand>
    {
        private readonly IApplicationDbContext _db;

        public RemoveRoleCommandHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Handle(
            RemoveRoleCommand request,
            CancellationToken cancellationToken)
        {
            var ur = await _db.UserRoles.FirstOrDefaultAsync(
                x => x.UserId == request.UserId
                     && x.RoleId == request.RoleId,
                cancellationToken);

            if (ur == null) return;

            _db.UserRoles.Remove(ur);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
