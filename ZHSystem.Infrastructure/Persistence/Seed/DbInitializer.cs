using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZHSystem.Application.Common.Interfaces;
using ZHSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHSystem.Infrastructure.Persistence.Seed
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public DbInitializer(
            ApplicationDbContext context,
            IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            await SeedRolesAsync();
            await SeedAdminAsync();
        }

        private async Task SeedRolesAsync()
        {
            if (await _context.Roles.AnyAsync())
                return;

            _context.Roles.AddRange(
                new Role { Name = "Admin" },
                new Role { Name = "User" }
            );

            await _context.SaveChangesAsync();
        }

        private async Task SeedAdminAsync()
        {
            if (await _context.Users.AnyAsync(u => u.Email == "admin@ZHSystem.com"))
                return;

            var adminUser = new User
            {
                UserName = "admin",
                Email = "admin@ZHSystem.com",
                isActive = true,
                EmailVerified = true,
                
            };

            adminUser.PasswordHash =
                _passwordHasher.HashPassword(adminUser, "asd@123");

            _context.Users.Add(adminUser);
            await _context.SaveChangesAsync();

            var adminRole = await _context.Roles
                .FirstAsync(r => r.Name == "Admin");

            _context.UserRoles.Add(new UserRole
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            });

            await _context.SaveChangesAsync();
        }
    }
}
