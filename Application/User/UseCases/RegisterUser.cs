using Application.CookieService;
using Application.User.Models;
using Contracts.ExternalContracts;
using Contracts.ExtrernalContracts;
using Domain.AuthenticationTokens;
using Domain.Tables;
using Domain.Utilites;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.UseCases
{
    public class RegisterUser
        (
        IUserManager<IdentityUser> userManager,
        IEmailSender  emailSender,
        ITokenProvider tokenProvider
        )
    {
        public record Request(RegisterDto user ,HttpContext context);

        public async Task<Result<AuthModel>> Handle(Request request)
        {
            var user  = await userManager.FindByEmailAsync(request.user.Email);
            if(user is not null)
            {
                return new Result<AuthModel>
                {
                    IsSuccess = false,
                    Message = "User Already Exists"
                };
            }
            user = new IdentityUser
            {
                Email=  request.user.Email,
                Username = request.user.Username,
                PasswordHash = userManager.Hash(request.user.Password),
                PhoneNumber = request.user.Phone
            };
            var refreshToken = tokenProvider.GenerateRefreshToken();
            user.RefreshTokens.Add(refreshToken);
            await userManager.Save();
            Cookies.SetCookie("refreshToken",
                refreshToken.ExpiryDate,
                refreshToken.Token,
                request.context);
            await emailSender.SendEmailAsync(user.Email,"Welcome", "Welcome to Identity Provider");
            return new Result<AuthModel>
            {
                IsSuccess = true,
                Data = new AuthModel
                {
                    AccessToken = tokenProvider.GenerateAccessToken(user),
                    RefreshTokenExpirationDate= user.RefreshTokens.Single().ExpiryDate
                }
            };
        }
    }
}
