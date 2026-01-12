    using Microsoft.EntityFrameworkCore;
    using ZHSystem.Application.Common;
    using ZHSystem.Domain.Entities;
    using System.Linq.Expressions;
    using System.Reflection;


    namespace ZHSystem.Infrastructure.Persistence
    {
        public class ApplicationDbContext : DbContext, IApplicationDbContext 
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            {

            }
            public DbSet<Student> Students => Set<Student>();
            public DbSet<User> Users => Set<User>();
            public DbSet<RefreshToken> RefreshTokens => Set < RefreshToken >();
            public DbSet<Role> Roles  => Set<Role>();
            public DbSet<UserRole> UserRoles  => Set<UserRole>();
            public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
          public Task<int> SaveChangesAsync(CancellationToken cancellationToken =default)
            {
                return base.SaveChangesAsync(cancellationToken);
            }

           
          
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

               
                modelBuilder.Entity<Student>().HasQueryFilter(s => !s.IsDeleted);
                modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
                modelBuilder.Entity<Role>().HasQueryFilter(r => !r.IsDeleted);

                
                modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            }
        }
    }
