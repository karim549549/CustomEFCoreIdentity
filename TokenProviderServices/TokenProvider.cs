using Contracts;
using Domain.AuthenticationTokens;
using Domain.Tables;
using Domain.Utilites;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Contracts.ExternalContracts;
using Microsoft.Extensions.Options;

namespace TokenProviderServices
{
    public sealed class TokenProvider
        (
        IOptions<JWT> _jwt
        ) : ITokenProvider
    {
        private JWT  jwt  =  _jwt.Value;
        public AccessToken GenerateAccessToken(IdentityUser user,
            List<string>? Roles = default)
        {
            try
            {
                var roleClaims = new List<Claim>();
                if (Roles is not null)
                {
                    foreach (var role in Roles)
                    {
                        roleClaims.Add(new Claim("Role", role));
                    }
                }

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            }.Union(roleClaims);

                if (string.IsNullOrEmpty(jwt.Key))
                {
                    throw new ArgumentNullException("JWT Key is null or empty.");
                }
                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));

                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: jwt.Issuer,
                    audience: jwt.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(jwt.Expiry),
                    signingCredentials: signingCredentials);

                var token = new AccessToken
                {
                    token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    Expiration = DateTime.Now.AddDays(jwt.Expiry),
                    Roles = Roles
                };

                return token;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public RefreshToken GenerateRefreshToken()
        {
            var token = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(token);
            }
            return new RefreshToken{Token = Convert.ToBase64String(token)};
        }

    }
}
