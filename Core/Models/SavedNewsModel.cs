using Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class SavedNewsModel : IdModel
    {
        public Guid NewsId { get; set; }
        public Guid? UserId { get; set; }
        public string? Title { get; set; }
        public string? UserName { get; set; }
        public DateTime? ViewedOn { get; set; }
    }
}
