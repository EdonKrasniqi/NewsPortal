using Core.Entities;
using Core.Entities.Base;
using Core.Enums;
using Core.Interfaces;
using System;

namespace Domain.Entities
{
    public class ReactionEntity : BaseIdEntity, IUserNews
    {
        public Guid UserId { get; set; }
        public Guid NewsId { get; set; }
        public ReactionTypes ReactionType { get; set; }
        public AppUserEntity User { get; set; }
        public NewsEntity News { get; set; }


    }
}
