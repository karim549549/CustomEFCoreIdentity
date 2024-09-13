
namespace Domain.Tables
{
    public class UserRoles
    {
        public Guid Id  { get; set; } 
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public DateTime  Created_At { get; set; }
        public DateTime  Updated_At { get; set; }
    }
}
