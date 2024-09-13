using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User
{
    public record  RegisterDto
        (
        string Email,
        string Password,
        string Username,
        string Phone
        );

    public record LoginDto(string Email , string Password);
    public record ResetPasswordDto(string NewPassword,
        string ConfirmPassword);
}
