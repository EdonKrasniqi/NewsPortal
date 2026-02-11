using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class UserRoleEntity : IdentityUserRole<Guid>
    {
        public override Guid UserId { get; set; }

        public override Guid RoleId { get; set; }
        public AppUserEntity User { get; set; }
        public RolesEntity Role { get; set; }

    }
}
