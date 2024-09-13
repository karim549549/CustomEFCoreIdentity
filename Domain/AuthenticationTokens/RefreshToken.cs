using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AuthenticationTokens

{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; } = DateTime.Now.AddDays(1);
        public bool IsExpired => DateTime.UtcNow > ExpiryDate;
        public bool IsActive => Revoked_At is null && !IsExpired;
        public DateTime Created_At { get; set; } = DateTime.Now;
        public DateTime? Revoked_At { get; set; }
    }
}
