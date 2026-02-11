using Core.Entities;
using Core.Entities.Base;
using System;

namespace Domain.Entities
{
    public class SiteConfigsEntity : BaseIdEntity
    {
        public Guid? HeaderLogoId { get; set; }
        public FileEntity? HeaderLogo { get; set; }
        public Guid? UpdatedById { get; set; }
        public Guid? FooterLogoId { get; set; }
        public FileEntity? FooterLogo { get; set; }
        public string HeaderColor { get; set; }
        public string FooterColor { get; set; }
        public AppUserEntity UpdatedBy { get; set; }
        public DateTime LatUpdatedOn { get; set; }

    }
}