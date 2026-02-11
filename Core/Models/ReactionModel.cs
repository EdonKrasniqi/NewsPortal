using Core.Enums;
using Core.Models.Base;
using System;

namespace Core.Models
{
    public class ReactionModel : IdModel
    {
        public Guid? UserId { get; set; }
        public Guid NewsId { get; set; }
        public ReactionTypes ReactionType { get; set; }
        public string? UserName { get; set; }
        public string? Title { get; set; }
        public int? Angry { get; set; }
        public int? Happy { get; set; }
        public int? Sad { get; set; }
    }
}
