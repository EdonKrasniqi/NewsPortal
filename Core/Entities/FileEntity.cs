using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class FileEntity: BaseIdEntity
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Extension { get;set; }
    }
}
