namespace Domain.Tables
{
    public class VerificationToken
    {
        public Guid Id { get; set; }= Guid.NewGuid();
        public string UserID { get; set; }
        public IdentityUser User { get; set; }
        public DateTime Created_At { get; set; } = DateTime.Now;
        public DateTime  Expires_At { get; set; } =DateTime.Now.AddHours(1);
    }
}
