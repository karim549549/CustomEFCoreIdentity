

using Contracts.ExternalContracts;
using Domain.Tables;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public sealed class RoleManager<Entity>
        (CustomIdentityDbContext<Entity> context)
        : IRoleManager<Entity>
        where Entity : IdentityUser
    {
        public async Task AddRole(Role role)
        {
            await context.Roles.AddAsync(role);
            await context.SaveChangesAsync();
        }
        public async Task<List<string>?> GetUserRoles(string UserId)
        {
            var userRoles = await context.UserRoles
                .Where(ur => ur.UserId == UserId)
                .Select(ur => ur.Role.Name)
                .ToListAsync();
            return userRoles;
        }
        public  async Task RemoveRole(Role role)
        {
            context.Remove(role);
            await context.SaveChangesAsync();
        }
        public async Task RemoveUserRole(string UserID, Guid RoleId)
        {
            var UserRole = await context.UserRoles
                    .Where(x => x.UserId == UserID && x.RoleId == RoleId)
                    .FirstOrDefaultAsync();
            context.UserRoles.Remove(UserRole);
        }
    }
}
