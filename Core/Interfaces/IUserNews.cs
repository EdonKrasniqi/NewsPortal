using Core.Entities;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface IUserNews
    {
        Guid UserId { get; set; }
        AppUserEntity User { get; set; }

        public Guid NewsId { get; set; }
        public NewsEntity News { get; set; }
    }
}
