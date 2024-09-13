using Domain.AuthenticationTokens;
using Domain.Tables;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class CustomIdentityDbContext<TUser> 
        (DbContextOptions<CustomIdentityDbContext<TUser>> options) 
        : DbContext(options)
        where TUser : IdentityUser
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TUser>(u=>{
                u.OwnsMany<RefreshToken>("RefreshTokens");
                u.HasIndex(u => u.Email).IsUnique();
            });

            modelBuilder.Entity<UserRoles>(ur => { 
                ur.HasIndex(ur => new { ur.UserId, ur.RoleId }).IsUnique();
            }); 
        }
        public DbSet<TUser> Users { get; set; }
        public DbSet <UserRoles> UserRoles { get; set; }
        public DbSet <VerificationToken>  verificationTokens { get; set; }
        public DbSet <Role>  Roles{ get; set; }

    }


}
