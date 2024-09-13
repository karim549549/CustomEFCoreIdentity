using Contracts.ExtrernalContracts;
using Domain.Utilites;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Mail;
namespace EmailSenderService
{
    public static class EmailServiceExtension
    {
        public static IServiceCollection AddEmailService(
            this IServiceCollection services,
            SmtpSetting smtpSettings)
        {
            services
                .AddFluentEmail(smtpSettings.User, "IdentityProvider")
                .AddSmtpSender(() => new SmtpClient(smtpSettings.Host)
                {
                    Port = smtpSettings.Port,
                    Credentials = new NetworkCredential(smtpSettings.User, smtpSettings.Password),
                    EnableSsl = true,
                });
            services.AddScoped<IEmailSender, EmailSender>();
            return services;
        }
    }
}
