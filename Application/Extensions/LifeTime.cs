using Application.User.UseCases;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class LifeTime
    {
        public static IServiceCollection AddServicesLifeTime(
            this IServiceCollection services)
        {

            services.AddScoped<LoginUser>();
            services.AddScoped<ForgetPassword>();
            services.AddScoped<ResetPassword>();
            services.AddScoped<RegisterUser>();
            services.AddScoped<VerifyEmail>();
            services.AddScoped<SendVerificationEmail>();
            return services;
        }
    }
}
