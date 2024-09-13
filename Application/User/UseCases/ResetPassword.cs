using Contracts.ExternalContracts;
using Contracts.ExtrernalContracts;
using Domain.AuthenticationTokens;
using Domain.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.UseCases
{
    public class ResetPassword
        (
        IUserManager<IdentityUser> userManager,
        IEmailSender emailSender
        )
    {
        public record Request(string NewPassword , Guid VerificationTokenId);
        public async Task<bool> Handle(Request request)
        {
            var verificationToken = await userManager.GetVerificationToken(request.VerificationTokenId);
            if (verificationToken is null ||
                verificationToken.Expires_At < DateTime.Now ||
                verificationToken.User.IsEmailConfirmed)
            {
                return false;
            }
            verificationToken.User.PasswordHash =
                userManager.Hash(request.NewPassword);
            userManager.DeleteVerificationToken(verificationToken);
            await userManager.Save();
            return true;
        }
    }
}
