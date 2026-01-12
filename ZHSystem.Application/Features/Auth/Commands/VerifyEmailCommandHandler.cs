using MediatR;
using Microsoft.EntityFrameworkCore;
using ZHSystem.Application.Common;
using ZHSystem.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZHSystem.Application.Features.Auth.Commands
{
    public record VerifyEmailCommand(string Token)
        : IRequest;
    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand>
    {
        private readonly IApplicationDbContext _db;

        public VerifyEmailCommandHandler(IApplicationDbContext db)
        {
            _db = db;
        }
        public async Task Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var hashedToken =
                Convert.ToBase64String(
                    SHA256.HashData(Encoding.UTF8.GetBytes(request.Token))
                );

            var user = await _db.Users.FirstOrDefaultAsync(u =>
                    u.EmailVerificationTokenHash == hashedToken &&
                    u.EmailVerificationExpires > DateTime.UtcNow,
                cancellationToken);

           
            if (user == null || user.EmailVerified)
                throw new BadRequestException("Invalid or expired token.");

            user.EmailVerified = true;
            user.EmailVerificationTokenHash = null;
            user.EmailVerificationExpires = null;

            await _db.SaveChangesAsync(cancellationToken);

            return;
        }
    }
}
