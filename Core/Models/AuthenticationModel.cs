using Core.Models.Base;
using System;

namespace Portal.Models
{
    public class AuthenticationModel : IdModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireDate { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserRole { get; set; }


    }
}
