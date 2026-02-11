using Core.Entities;
using Core.Models.Base;
using Domain.Entities;
using System;

namespace Core.Models
{
    public class NewsModel : IdModel
    {
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsDeleted { get; set; }
        public string Content { get; set; }
        public Guid ImageId { get; set; }
        public string Image { get; set; }
        public string Video { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? UpdatedById { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public DateTime UpdatedOnDate { get; set; }
        public string Tags { get; set; }
        public bool isSaved { get; set; }
    }
}
