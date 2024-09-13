using Domain.Utilites;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class AuthenticationSerivce
    {
        public static IServiceCollection AddAuth(
            this IServiceCollection services,
            IConfigurationSection jwtSection)
        {
            var jwtOptions = jwtSection.Get<JWT>();

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.TryGetValue("jwt", out var token))
                        {
                            if (!string.IsNullOrEmpty(token))
                            {
                                context.Token = token;
                                Console.WriteLine($"Token found in cookie: {token}");
                            }
                        }
                        return Task.CompletedTask;
                    }
                };
                //options.Events = new JwtBearerEvents
                //{
                //    OnMessageReceived = context =>
                //    {
                //        if (context.Request.Cookies.TryGetValue("jwt", out var token))
                //        {
                //            Console.WriteLine($"Token found in cookie: {token}");
                //            context.Token = token;
                //        }
                //        else
                //        {
                //            Console.WriteLine("No token found in cookie.");
                //        }
                //        return Task.CompletedTask;
                //    }
                //};
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ADMIN", policy => policy.RequireClaim("ADMIN"));
                options.AddPolicy("USER", policy => policy.RequireClaim("USER"));
            }
                );
            return services;
        }
    }
}
