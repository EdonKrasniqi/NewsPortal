using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class RolesEntity : IdentityRole<Guid>
    {
        public List<UserRoleEntity> UserRoles { get; set; }

    }
}
