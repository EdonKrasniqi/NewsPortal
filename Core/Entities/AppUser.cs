using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class AppUserEntity : IdentityUser<Guid>
    {
        public Guid? AvatarId { get; set; }
        public FileEntity? Avatar { get; set; }
        public List<UserRoleEntity> UserRoles { get; set; }

    }
}
