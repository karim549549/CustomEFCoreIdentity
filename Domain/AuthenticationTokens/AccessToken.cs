

namespace Domain.AuthenticationTokens
{
    public class AccessToken
    {
        public string token { get; set; }
        public List<string> Roles { get; set; }
        public DateTime Expiration { get; set; }
    }
}
