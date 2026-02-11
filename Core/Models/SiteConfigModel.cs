using Core.Models.Base;
using System;

namespace Core.Models
{
    public class SiteConfigurationModel : IdModel
    {
        public Guid? HeaderLogoId { get; set; }
        public string? HeaderLogo { get; set; }
        public Guid? FooterLogoId { get; set; }
        public string? FooterLogo { get; set; }
        public string HeaderColor { get; set; }
        public string FooterColor { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime LatUpdatedOn { get; set; }
    }
}
