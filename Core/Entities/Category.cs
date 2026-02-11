using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class CategoryEntity : BaseIdEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ShowOnline { get; set; }
        public List<NewsEntity> News { get; set; } = new List<NewsEntity>();
    }
}