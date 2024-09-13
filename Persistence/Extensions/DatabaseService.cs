using Contracts.ExternalContracts;
using Domain.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Extensions
{
    public static class DatabaseService
    {
        public static IServiceCollection AddDataBase(
            this IServiceCollection services,
            string ConnectionString)
        {
            services.AddDbContext<CustomIdentityDbContext<IdentityUser>>(options =>
            {
                options.UseSqlServer(
                    ConnectionString ??
                    throw new Exception("Connection string 'DefaultConnection' not found."));
            });
            services.AddScoped(typeof(IUserManager<>), typeof(UserManager<>));
            services.AddScoped(typeof(IRoleManager<>), typeof(RoleManager<>));
            return services;
        }
    }
}
