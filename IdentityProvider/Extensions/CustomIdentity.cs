
using Domain.Utilites;
using EmailSenderService;
using Persistence.Extensions;
using Application.Extensions;
using TokenProviderServices;
namespace IdentityProvider.Extensions
{
    public static class CustomIdentity
    {
        public static IServiceCollection AddCustomIdentity(
            this IServiceCollection Services,
            IConfiguration  Configuration)
        {
            Services.AddAuth(Configuration.GetRequiredSection("JWT"));
            Services.AddServicesLifeTime();
            Services.AddEmailService(
                Configuration.GetSection("SmtpSettings")
                .Get<SmtpSetting>() ?? throw new Exception("SmtpSettings not found.")
                );
            Services.AddDataBase(
                Configuration.GetConnectionString("DefaultConnection")
                ?? throw new Exception("Connection string 'DefaultConnection' not found.")
                );
            Services.Configure<JWT>(Configuration.GetSection("JWT"));
            Services.AddTokenProviderLifeTime();
            return Services;
        }
    }
}
