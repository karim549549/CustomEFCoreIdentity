using Contracts.ExternalContracts;

using Microsoft.Extensions.DependencyInjection;

namespace TokenProviderServices
{
    public static class TokenServiceExtensions
    {
        public static IServiceCollection AddTokenProviderLifeTime(
            this IServiceCollection services)
        {
            services.AddScoped<ITokenProvider, TokenProvider>();
            return services;
        }
    }
}
