using Core.Models.Base;

namespace Core.Models
{
    public class BaseUserModel : IdModel
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string PhoneNumber { get; set; }
    }
}
