using Domain.AuthenticationTokens;
using Domain.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.ExternalContracts
{
    public interface IUserManager<Entity>
        where Entity : IdentityUser
    {
        public Task<IdentityUser?> FindByEmailAsync(string email);
        //public Task InsertUserAsync(Entity user);
        public bool Compare(string Password, string PasswordHash);
        public string Hash(string Password);
        public Task AddUserRole(string UserId, Guid RoleId);
        //public Task ResetPassword(Entity user, string Password);
        //public Task VerifyUserEmail(Entity user);
        //public Task InsertRefreshToken(Entity User,RefreshToken token);
        public Task<VerificationToken?> GetVerificationToken(Guid token);
        public Task InsertVerificationToken(VerificationToken token);
        public Task Save();
        public void DeleteVerificationToken(VerificationToken token);
        public Task<Entity?> GetUserByRefreshToken(string Token);
    }
}
