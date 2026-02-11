using Core.Entities;
using Core.Models.Base;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class ViewModel : IdModel
    {
        public Guid? UserId { get; set; } = default!;
        public string FingerPrintId { get; set; }
        public Guid NewsId { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }
        public int NumberOfViews { get; set; }
        public DateTime ViewedOn { get; set; }
    }
}
