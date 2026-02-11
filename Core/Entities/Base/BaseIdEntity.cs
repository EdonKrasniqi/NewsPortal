using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Base
{
    public class BaseIdEntity : IBaseEntity
    {
        public Guid Id { get;set; }
    }
}
