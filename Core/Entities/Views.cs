using Core.Entities;
using Core.Entities.Base;
using Core.Interfaces;
using System;

namespace Domain.Entities
{
    public class ViewsEntity : BaseIdEntity
    {
        public Guid? UserId { get; set; }
        public string? FingerPrintId { get; set; }
        public Guid NewsId { get; set; }
        public NewsEntity News { get; set; }
        public AppUserEntity? User { get; set; }
        public DateTime ViewedOn { get; set; }

    }
}