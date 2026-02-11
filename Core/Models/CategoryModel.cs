using Core.Models.Base;
using System;
namespace Core.Models
{
    public class CategoryModel : IdModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ShowOnline { get; set; }
    }
}
