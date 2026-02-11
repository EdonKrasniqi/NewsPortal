using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Portal.Models
{
    public class LoginModel
    {
        public string? Id { get; set; } = default!;
        public string Email { get; set; }
        public string Password { get; set; }
        public string FingerPrintId { get; set; }
        public bool RememberMe { get; set; }
    }
}
