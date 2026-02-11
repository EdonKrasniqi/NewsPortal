using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public  class HomePageConfigEntity : BaseIdEntity
    {
        public bool ShowFeatured { get; set; }
        public bool ShowAds { get; set; }
        public Guid? UpdatedById { get; set; }
        public bool ShowMostViewed { get; set; }
        public bool ShowLastViewed { get; set; }
        public AppUserEntity UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}
