using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Portal.Models
{
    public class EditUserModel : BaseUserModel
    {
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}