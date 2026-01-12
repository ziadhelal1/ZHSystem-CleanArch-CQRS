using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ZHSystem.Application.Common;
using ZHSystem.Application.Common.Exceptions;
using ZHSystem.Application.Common.Interfaces;
using ZHSystem.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHSystem.Application.Features.Auth.Commands
{
    public record RefreshTokenCommand(string RefreshToken): IRequest<RefreshResponseDto>
    {
    }
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshResponseDto>
    {
        private readonly IApplicationDbContext _db;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwt;

        public RefreshTokenCommandHandler(
            IApplicationDbContext db,
            ITokenService tokenService,
            IOptions<JwtSettings> jwt)
        {
            _db = db;
            _tokenService = tokenService;
            _jwt = jwt.Value;
        }
        public async Task<RefreshResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {

            var storedToken = await _db.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken, cancellationToken);

            if (storedToken == null ||
                storedToken.IsRevoked ||
                storedToken.Expires < DateTime.UtcNow)
                throw new BadRequestException("Invalid refresh token");

            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Id == storedToken.UserId, cancellationToken);

            if (user == null)
                throw new NotFoundException("User Not Exist");

            var roles = await _db.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.Role.Name)
                .ToListAsync(cancellationToken);

            var newAccessToken = _tokenService.GenerateAccessToken(user, roles);

            storedToken.IsRevoked = true;
            var newRefreshToken = _tokenService.CreateRefreshToken(user.Id);
            _db.RefreshTokens.Add(newRefreshToken);

            await _db.SaveChangesAsync(cancellationToken);

            return new RefreshResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwt.AccessTokenExpirationMinutes)
            };
        }
    }
}
