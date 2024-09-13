using Domain.AuthenticationTokens;

namespace Domain.Tables
{
    public class IdentityUser
    {
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;
        public string PhoneNumber  { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; } = false;
        public string PasswordHash { get; set; }
        public DateTime  Created_At { get; set; } = DateTime.Now;
        public DateTime  Updated_At { get; set; } = DateTime.Now;
        public ICollection<UserRoles>? Roles { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }=
            new List<RefreshToken>();
    }
}
