
using Application.CookieService;
using Application.User.Models;
using Contracts.ExternalContracts;
using Contracts.ExtrernalContracts;
using Domain.AuthenticationTokens;
using Domain.Tables;
using Domain.Utilites;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Application.User.UseCases
{
    public class LoginUser
        (
        IUserManager<IdentityUser> userManager,
        ITokenProvider tokenProvider,
        IEmailSender emailSender,
        IOptions<JWT> jwt
        )
    {
        public record Request(LoginDto user , HttpContext context);
        public async Task<Result<AuthModel>> Handle(Request request)
        {
            var user = await userManager.FindByEmailAsync(request.user.Email);
            if (user is null || !userManager.Compare(request.user.Password, user.PasswordHash))
            {
                return new Result<AuthModel>
                {
                    IsSuccess = false,
                    Message= "Invalid  Username or password"
                };
            }
            if (!user.RefreshTokens.Any(rf => rf.IsActive))
            {
                var refreshToken = tokenProvider.GenerateRefreshToken();
                user.RefreshTokens.Add(refreshToken);
                await userManager.Save();
                Cookies.SetCookie("refreshToken",
                    refreshToken.ExpiryDate,
                    refreshToken.Token,
                    request.context);
            }
            return new Result<AuthModel>
            {
                IsSuccess = true,
                Data = new AuthModel
                {
                    AccessToken = tokenProvider.GenerateAccessToken(user),
                    RefreshTokenExpirationDate = user.RefreshTokens.Single().ExpiryDate
                }
            };
        }
    }
}
