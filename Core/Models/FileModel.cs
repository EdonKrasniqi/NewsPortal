using Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class FileModel : IdModel
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
        public string Type { get; set; }
    }
}
