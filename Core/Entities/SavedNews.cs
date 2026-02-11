using Core.Entities;
using Core.Entities.Base;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class SavedNewsEntity : BaseIdEntity, IUserNews
    {
        public Guid NewsId { get; set; }
        public Guid UserId { get; set; }
        public NewsEntity News { get; set; }
        public AppUserEntity User { get; set; }
    }
}