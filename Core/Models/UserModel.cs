using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.ViewModels
{
    public class UserModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string? PhoneNumber { get; set; } = default!;
        public Guid? AvatarId { get; set; } = default!;

    }
}
