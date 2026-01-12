using MediatR;
using ZHSystem.Application.Common;
using ZHSystem.Application.Common.Interfaces;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ZHSystem.Application.Common.Exceptions;

namespace ZHSystem.Application.Features.Auth.Commands
{
    public record ForgotPasswordCommand(string Email):IRequest;
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly IApplicationDbContext _db;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public ForgotPasswordCommandHandler(
            IApplicationDbContext db,
            IEmailService emailService ,IConfiguration Configuration)
        {
            _db = db;
            _emailService = emailService;
            _configuration = Configuration;
        }
        public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
            if (user == null)
                return ;
            //  rate limit => 5 minutes

            if (user.LastSecurityEmailSentAt.HasValue)
            {
                var nextAllowedSendTime = user.LastSecurityEmailSentAt.Value.AddMinutes(5);
                if (nextAllowedSendTime > DateTime.UtcNow)
                {

                    var remainingSeconds = (int)(nextAllowedSendTime - DateTime.UtcNow).TotalSeconds;
                    throw new RateLimitException($"Please wait {remainingSeconds} seconds before requesting another email.");
                }
            }
            var rawToken = Guid.NewGuid().ToString("N");
           
            user.PasswordResetTokenHash=Convert.ToBase64String(
                    SHA256.HashData(Encoding.UTF8.GetBytes(rawToken))
                );
            user.PasswordResetExpires = DateTime.UtcNow.AddMinutes(30);
            user.LastSecurityEmailSentAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(cancellationToken);
            var baseUrl = _configuration["ClientSettings:BaseUrl"];
            var resetUrl = $"{baseUrl}/auth/reset-password?token={rawToken}"; 
            var html = $@"
                        <h2>Reset Password</h2>
                        <p>Click the link below to reset your password:</p>
                        <a href='{resetUrl}'>Reset Password</a>
                        <p>This link expires in 30 minutes.</p>";

                await _emailService.SendAsync(
                    user.Email,
                    "Reset your password",
                    html
                );

                return;

        }
    }
}
