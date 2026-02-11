using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class NewsDetailsConfigEntity : BaseIdEntity
    {
        public string FontColor { get; set; }
        public string BackgroundColor { get; set; }
        public Guid? BackgroundImgId { get; set; }
        public FileEntity? BackgroundImg { get; set; }
        public bool ShowVideoAsLink { get; set; }
        public bool ShowRelated { get; set; }
        public int RelatedNumbers { get; set; }
        public Guid? UserId { get; set; }
        public AppUserEntity? User { get; set; }
    }
}
