using Domain.AuthenticationTokens;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CookieService
{
    public static class Cookies
    {
        public static void SetCookie(string SetIn 
            ,DateTime Expire 
            , string Data
            , HttpContext context)
        {
            var CookieOption = new CookieOptions
            {
                HttpOnly = true,
                Expires = Expire,
                SameSite = SameSiteMode.Strict,
                Secure = true
            };
            context.Response.Cookies.Append( SetIn,  Data);
        }
    }
}
