using Contracts.ExternalContracts;
using Domain.Tables;

namespace Application.User.UseCases
{
    public class VerifyEmail(
        IUserManager<IdentityUser> userManager
        )
    {
        public record  Request(Guid VerificationTokenId);   
        public async Task<bool> Handle(Request request)
        {
            var verificationToken = await userManager.GetVerificationToken(request.VerificationTokenId);
            if (verificationToken is null
                || verificationToken.Expires_At < DateTime.UtcNow
                || verificationToken.User.IsEmailConfirmed)
            {
                return false;
            }
            verificationToken.User.IsEmailConfirmed = true;
            userManager.DeleteVerificationToken(verificationToken);
            await userManager.Save();
            return true;
        } 
    }
}
