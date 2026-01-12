using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ZHSystem.Application.Common;
using ZHSystem.Application.DTOs.UserMangment;
using ZHSystem.Domain.Entities;

namespace ZHSystem.Application.Features.Users.Commands
{
    public record AssignRoleCommand(AssignRoleDto dto) : IRequest;
    public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand>
    {
        private readonly IApplicationDbContext _db;
        public AssignRoleCommandHandler(IApplicationDbContext db)
        {
            _db = db;
        }
        public async Task Handle(AssignRoleCommand request, CancellationToken cancellationToken)
        {
            var exist=_db.UserRoles
                .Any(ur=>ur.UserId==request.dto.UserId && ur.RoleId==request.dto.RoleId);
            if (exist) return ;
            _db.UserRoles.Add(new UserRole
            {
                UserId = request.dto.UserId,
                RoleId = request.dto.RoleId
            });
            await _db.SaveChangesAsync(cancellationToken);
            
        }
    }
}
