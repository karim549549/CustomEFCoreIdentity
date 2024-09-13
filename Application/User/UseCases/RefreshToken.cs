using Application.User.Models;
using Contracts.ExternalContracts;
using Domain.Tables;
using Domain.Utilites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.UseCases
{
    public class RefreshToken
        (
        IUserManager<IdentityUser> userManager,
        ITokenProvider tokenProvider
        )
    {
        public record Request(string RefreshToken );

        public async Task<Result<AuthModel>> Handle(Request request)
        {
            var user = await userManager.GetUserByRefreshToken(request.RefreshToken);
            var oldToken = user.RefreshTokens.Single();
            if(user is null || !oldToken.IsActive)
            {
                return new Result<AuthModel>
                {
                    IsSuccess = false,
                    Message = "Invalid Token"
                };
            }
            oldToken.Revoked_At = DateTime.Now;
            var newRefreshToken = tokenProvider.GenerateRefreshToken();
            user.RefreshTokens.Add( newRefreshToken );
            await userManager.Save();
            return new Result<AuthModel>
            {
                IsSuccess = true,
                Data = new AuthModel
                {
                    AccessToken = tokenProvider.GenerateAccessToken(user),
                    RefreshTokenExpirationDate = newRefreshToken.ExpiryDate,
                }
            }; 
        }
    }
}
