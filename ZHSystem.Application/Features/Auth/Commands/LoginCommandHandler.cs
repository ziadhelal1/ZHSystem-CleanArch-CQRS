using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ZHSystem.Application.Common;
using ZHSystem.Application.Common.Exceptions;
using ZHSystem.Application.Common.Interfaces;
using ZHSystem.Application.DTOs.Auth;
using ZHSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHSystem.Application.Features.Auth.Commands
{
    public record LoginCommand(LoginDto Dto)
        : IRequest<RefreshResponseDto>;
    public class LoginCommandHandler : IRequestHandler<LoginCommand, RefreshResponseDto>
    {
        private readonly IApplicationDbContext _db;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwt;

        public LoginCommandHandler(
            IApplicationDbContext db,
            IPasswordHasher<User> passwordHasher,
            ITokenService tokenService,
            IOptions<JwtSettings> jwt)
        {
            _db = db;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _jwt = jwt.Value;
        }
        public async Task<RefreshResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var user = await _db.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email, cancellationToken);

            if (user == null)
                throw new UnauthorizedException("Invalid credentials");

            if (!user.EmailVerified)
                throw new ForbiddenException("Email not verified");

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedException("Invalid credentials");

            var roles = user.UserRoles
                .Select(ur => ur.Role.Name)
                .ToList();

            var accessToken = _tokenService.GenerateAccessToken(user, roles);

            var refreshToken = _tokenService.CreateRefreshToken(user.Id);
            await _db.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            return new RefreshResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwt.AccessTokenExpirationMinutes)
            };
        }
    }
}
