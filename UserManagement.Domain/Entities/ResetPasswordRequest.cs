using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Domain.Entities
{
    public class ResetPasswordRequest
    {
        public string? Email { get; set; }
        public string? NewPassword { get; set; }
        public string? ResetToken { get; set; } // optional, if you implement token-based reset

    }
}
