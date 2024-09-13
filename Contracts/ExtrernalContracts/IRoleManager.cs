using Domain.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.ExternalContracts
{
    public interface IRoleManager<Entity>
        where Entity : IdentityUser
    {
        public Task<List<string>?> GetUserRoles(string UserId);
        public Task RemoveUserRole(string UserID, Guid RoleId);
        public Task AddRole(Role role);
        public Task RemoveRole(Role role);
    }
}
