using Contracts.ExternalContracts;
using Contracts.ExtrernalContracts;
using Domain.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.UseCases
{
    public class ForgetPassword
        (
        IUserManager<IdentityUser> userManager,
        IEmailSender emailSender
        )
    {
        public record Request(string Email);

        public async Task<bool> Handle(Request request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return false;
            }
            var verificationToken = new VerificationToken { UserID = user.Id };
            await userManager.InsertVerificationToken(verificationToken);
            await userManager.Save();
            //add VerificationTokenId to the VerificatonLink to be implemented later
            await emailSender.SendEmailAsync(
                user.Email,
                "Forget Password Request",
                $"click on link  {verificationToken.Id}"
                );
            return true;
        }
    }
}
