using Contracts.ExternalContracts;
using Contracts.ExtrernalContracts;
using Domain.Tables;


namespace Application.User.UseCases
{
    public class SendVerificationEmail
        (
        IUserManager<IdentityUser> userManager,
        IEmailSender emailSender
        )
    {
        public record Request(string Email  , string UserId);
        public async Task<bool> Handle(Request request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if(user is null)
            {
                return false;
            }
            var verificationToken = new VerificationToken {UserID = request.UserId };
            await userManager.InsertVerificationToken(verificationToken);
            var verificationLink = $"click here {verificationToken}";
            await emailSender.SendEmailAsync(
                request.Email 
                ,"VerifyEmail"
                ,verificationLink);
            await userManager.Save();
            return true;
        }
    }
}

