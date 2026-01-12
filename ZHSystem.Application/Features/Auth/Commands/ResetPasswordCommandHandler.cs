using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZHSystem.Application.Common;
using ZHSystem.Application.Common.Exceptions;
using ZHSystem.Domain.Entities;

namespace ZHSystem.Application.Features.Auth.Commands
{
    public record ResetPasswordCommand(
        string Token,
        string NewPassword
    ) : IRequest;
    public class ResetPasswordCommandHandler
        : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IApplicationDbContext _db;
        private readonly IPasswordHasher<User> _passwordHasher;

        public ResetPasswordCommandHandler(
            IApplicationDbContext db,
            IPasswordHasher<User> passwordHasher)
        {
            _db = db;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(
            ResetPasswordCommand request,
            CancellationToken cancellationToken)
        {
            var hashedToken =
                Convert.ToBase64String(
                    SHA256.HashData(
                        Encoding.UTF8.GetBytes(request.Token))
                );

            var user = await _db.Users.FirstOrDefaultAsync(u =>
                    u.PasswordResetTokenHash == hashedToken &&
                    u.PasswordResetExpires > DateTime.UtcNow,
                cancellationToken);

            if (user == null)
                throw new BadRequestException("Invalid or expired token");

            user.PasswordHash =
                _passwordHasher.HashPassword(user, request.NewPassword);

            user.PasswordResetTokenHash = null;
            user.PasswordResetExpires = null;

            await _db.SaveChangesAsync(cancellationToken);

            return ;
        }
    }
}
