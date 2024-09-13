
using Domain.Utilites;
using Domain.AuthenticationTokens;
using Domain.Tables;

namespace Contracts.ExternalContracts
{
    public interface ITokenProvider

    {
        public AccessToken GenerateAccessToken(IdentityUser user 
            , List<string>? Roles = default);
        public RefreshToken GenerateRefreshToken();
    }
}
