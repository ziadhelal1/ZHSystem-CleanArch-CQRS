using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZHSystem.Application.Common;
using ZHSystem.Application.Common.Interfaces;
using ZHSystem.Domain.Entities;
using ZHSystem.Infrastructure.Persistence;
using ZHSystem.Infrastructure.Persistence.Models;
using ZHSystem.Infrastructure.Persistence.Seed;
using ZHSystem.Infrastructure.Services;


namespace ZHSystem.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // 1. DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IApplicationDbContext>(sp =>
                sp.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IDbInitializer, DbInitializer>();

            // 2. JWT Settings
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

            // 3. Services
            services.AddScoped<ITokenService, TokenService>();
            services.Configure<SmtpSettings>(
                configuration.GetSection("SmtpSettings"));
            services.AddScoped<IEmailService, MailKitEmailService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            

            return services;
        }
    }
}