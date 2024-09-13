using Domain.AuthenticationTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.User.Models
{
    public class AuthModel
    {
        public AccessToken AccessToken { get; set; }
        public DateTime RefreshTokenExpirationDate { get; set; }    
    }
}
