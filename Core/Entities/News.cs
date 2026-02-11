using Core.Entities;
using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class NewsEntity : BaseIdEntity
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsDeleted { get; set; }
        public string Content { get; set; }
        public Guid ImageId { get; set; }
        public FileEntity Image { get; set; }
        public string Video { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryEntity Category { get; set; }
        public DateTime ExpireDate { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? UpdatedById { get; set; }
        public AppUserEntity UpdatedBy { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public DateTime UpdatedOnDate { get; set; }
        public AppUserEntity CreatedBy { get; set; }
        public string Tags { get; set; }
        public List<ViewsEntity> Views { get; set; } = new List<ViewsEntity>();
        public List<SavedNewsEntity> SavedNews { get; set; } = new List<SavedNewsEntity>();
        public List<ReactionEntity> Reactions { get; set; } = new List<ReactionEntity>();

    }
}
