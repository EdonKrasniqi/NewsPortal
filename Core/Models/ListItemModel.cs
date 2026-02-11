using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class ListItemModel
    {
        public ListItemModel() { }
        public ListItemModel(Guid id, string Name)
        {
            this.Id = id;
            this.Name = Name;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
