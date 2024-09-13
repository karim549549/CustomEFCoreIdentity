
using Contracts.ExtrernalContracts;
using FluentEmail.Core;

namespace EmailSenderService
{
    public sealed class EmailSender
        (IFluentEmail
        fluentEmail
        ) : IEmailSender
    {
         public async Task SendEmailAsync(string email,
            string subject, string message)
        {
            try
            {
                var emailToSend = fluentEmail
                    .SetFrom("01150067996k@gmail.com", "IdentityProvider")
                    .To(email)
                    .Subject(subject)
                    .Body(message);


                await emailToSend.SendAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
    }
}
