using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class SaveFileModel
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public byte[] Data { get; set; }
    }
}
