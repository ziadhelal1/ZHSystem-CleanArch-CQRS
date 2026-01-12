using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ZHSystem.Application.Common;
using ZHSystem.Application.Common.Exceptions;
using ZHSystem.Application.Common.Interfaces;
using ZHSystem.Application.DTOs;
using ZHSystem.Application.DTOs.Auth;
using ZHSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZHSystem.Application.Features.Auth.Commands
{
    public record RegisterCommand(RegisterDto Dto) : IRequest<RegisterResponseDto>;
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponseDto>
    {
        private readonly IApplicationDbContext _db;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public RegisterCommandHandler(
            IApplicationDbContext db,
            IPasswordHasher<User> passwordHasher,
            ITokenService tokenService, IMapper mapper
            ,IEmailService emailService, IConfiguration configuration)
        {
            _db = db;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _mapper = mapper;
            _emailService = emailService;
            _configuration = configuration;
        }
        public async Task<RegisterResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            if (await _db.Users.AnyAsync(u => u.Email == dto.Email, cancellationToken)) 
            {
                var errors = new Dictionary<string, string[]> { {
                    "Email" ,new[]{"This Email Already Registered."} } };
                throw new ValidationException(errors);

            }


            var user = _mapper.Map<User>(dto);
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
            user.EmailVerified = false;
            var rawToken = Guid.NewGuid().ToString("N");

            user.EmailVerificationTokenHash =
                Convert.ToBase64String(
                    SHA256.HashData(Encoding.UTF8.GetBytes(rawToken))
                );

            user.EmailVerificationExpires = DateTime.UtcNow.AddHours(24);
            const int StudentRoleId = 2;
            const string StudentRoleName = "User";

            var userRole = new UserRole { User = user, RoleId = StudentRoleId };
            await _db.Users.AddAsync(user, cancellationToken);
            await _db.UserRoles.AddAsync(userRole, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);


            var baseUrl = _configuration["ClientSettings:BaseUrl"];
            var verifyUrl= $"{baseUrl}/auth/verify-email?token={rawToken}";
            
            var htmlBody = $@"
                <h2>Welcome to ZHSystem System</h2>
                <p>Please verify your email by clicking the link below:</p>
                <a href='{baseUrl}'>Verify Email</a>
                ";

            await _emailService.SendAsync(
                user.Email,
                "Verify your email",
                htmlBody
            );

           
            return new RegisterResponseDto
            {
                Message = "Registration successful. Please verify your email."
            };



        }
    }
}
