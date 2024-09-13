using Application.User.UseCases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace Application.User.Endpoints
{
    public static  class IdentityEndpoints
    {
        public static IEndpointRouteBuilder MapIdentityEndpoints(
    this IEndpointRouteBuilder Group)
        {
            var Route = Group.MapGroup("users/");

            Route.MapPost("login", async ([FromBody] LoginDto user,
                [FromServices] LoginUser Handle ,
                HttpContext context) =>
            {
                var request = new LoginUser.Request(user , context);
                var result = await Handle.Handle(request);
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(new
                    {
                        Status = "Failed",
                        result.Message
                    });
                }
                return Results.Ok(new
                {
                    Status = "Success",
                    result.Message
                });
            });

            Route.MapPost("register", async ([FromBody] RegisterDto user,
                [FromServices] RegisterUser Handle ,
                HttpContext context) => 
            {
                var request = new RegisterUser.Request(user , context);
                var  result = await Handle.Handle(request);
                if (! result.IsSuccess)
                {
                    return Results.BadRequest(new
                    {
                        Status = "Failed",
                        result.Message
                    });
                }
                return Results.Ok(new
                {
                    Status =  "Success",
                    result.Data
                });
            });

            Route.MapPost("reset-password", async ([FromServices] ResetPassword  Handle,
                [FromQuery] Guid verificationTokenId,
                [FromBody] ResetPasswordDto password) =>
            {
                if(!(password.NewPassword == password.ConfirmPassword))
                {
                    return Results.BadRequest(new
                    {
                        Message= ""
                    });
                }
                var request = new ResetPassword.Request(password.NewPassword, verificationTokenId);
                var result = await Handle.Handle(request);
                if (!result)
                {
                    return Results.BadRequest(new
                    {
                        Message = ""
                    });
                }
                return Results.Ok(new
                {
                    Message = ""
                });
            });

            Route.MapGet("verify-email", async (
                [FromServices] VerifyEmail Handle
                ,[FromQuery] Guid verificationTokenId) => 
            {
                var request = new VerifyEmail.Request(verificationTokenId);
                var result = await Handle.Handle(request);
                if (! result)
                {
                    return Results.BadRequest(new
                    {
                        Message = ""
                    });
                }
                return Results.Ok(new
                {
                    Message = ""
                });
            });

            Route.MapGet("forget-password",async ([FromServices] ForgetPassword Handle,
                [FromBody] string Email) =>
            {
                var request = new ForgetPassword.Request(Email);
                var result = await Handle.Handle(request);
                if(!result)
                {
                    return Results.BadRequest(new
                    {
                        Message= ""
                    });
                }
                return Results.Ok(new
                {
                    Message = ""
                });
            });

            Route.MapGet("resend-verification-Email",
                async ([FromServices] SendVerificationEmail Handle ,HttpContext context) =>
            {
                var email = context.User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var userId = context.User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var request = new SendVerificationEmail.Request(email,userId);
                var result = await Handle.Handle(request);
                if (!result)
                {
                    return Results.BadRequest(new
                    {
                        Message = ""
                    });
                }
                return Results.Ok(new
                {
                    Message = ""
                });
            });

            Route.MapGet("refresh-token", async (HttpContext context,
                [FromServices] RefreshToken Handle) =>
            {
                var token = context.Request.Cookies["refreshToken"];
                var request = new RefreshToken.Request(token);
                var result = await Handle.Handle(request);
                if (!result.IsSuccess)
                {
                    return Results.BadRequest(new
                    {
                        result.Message
                    });
                }
                return Results.Ok(new
                {
                    Message = "Refresh Token Created Succussfully",
                    result.Data
                });
            });


            return Group;
        }
    }
}
