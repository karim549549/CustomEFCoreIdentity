using Contracts.ExternalContracts;
using Domain.AuthenticationTokens;
using Domain.Tables;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Persistence
{
    public sealed class UserManager<Entity>
        (
           CustomIdentityDbContext<Entity> context,
           ITokenProvider tokenProvider 
        ):IUserManager<Entity>
        where Entity : IdentityUser
    {
        public async Task AddUserRole(string UserId, Guid RoleId)
        {
            await context.UserRoles.AddAsync(
                new UserRoles { UserId = UserId, RoleId = RoleId });
            await context.SaveChangesAsync();

        }

        public bool Compare(string Password, string PasswordHash)
        {
            return BCrypt.Net.BCrypt.Verify(Password, PasswordHash);
        }
        public async Task<IdentityUser?> FindByEmailAsync(string email)
        {
            return await context.Users.FindAsync(email);
        }

        public string Hash(string Password)
        {
            return BCrypt.Net.BCrypt.HashPassword(Password);
        }
        public async Task InsertVerificationToken(VerificationToken token)
        {
            await context.verificationTokens.AddAsync(token);
        }
        public async Task<VerificationToken?> GetVerificationToken(Guid TokenId)
        {
            return await context.verificationTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == TokenId);
        }
        public async Task Save()
        {
            await  context.SaveChangesAsync();
        }
        public void DeleteVerificationToken(VerificationToken token)
        {
            context.verificationTokens.Remove(token);
        }
        public async Task<Entity?> GetUserByRefreshToken(string Token)
        {
            return await context.Users
                .SingleOrDefaultAsync(
                u => u.RefreshTokens
                .Any(t => t.Token == Token));
        }
    }
}
