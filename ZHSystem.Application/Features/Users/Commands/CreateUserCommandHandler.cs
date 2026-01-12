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
    public record CreateUserCommand(CreateUserDto userDto):IRequest<int>;
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IApplicationDbContext _db;
        private readonly IPasswordHasher<User> _hasher;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IApplicationDbContext db, IPasswordHasher<User> hasher,IMapper mapper
        )
        {
            _db = db;
            _hasher = hasher;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken ct)
        {
            var user = _mapper.Map<User>(request.userDto);
            
                
            user.PasswordHash = _hasher.HashPassword(user, request.userDto.Password);

            _db.Users.Add(user);
            await _db.SaveChangesAsync(ct);

            return user.Id;
        }
    }
}
