using MediatR;
using Microsoft.AspNetCore.Identity;
using ZHSystem.Application.Common;
using ZHSystem.Application.DTOs.UserMangment;
using ZHSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace ZHSystem.Application.Features.Users.Commands
{
    public record CreateRoleCommand(CreateRoleDto roleDto) : IRequest<int>;
    public class CreateRoleCommandHandler: IRequestHandler<CreateRoleCommand, int>
    {
        private readonly IApplicationDbContext _db;
        private readonly IMapper _mapper;
        
        public CreateRoleCommandHandler(IApplicationDbContext db ,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = _mapper.Map<Role>(request.roleDto);
            
            _db.Roles.Add(role);
            await _db.SaveChangesAsync(cancellationToken);
            return role.Id;
        }
    }
}
