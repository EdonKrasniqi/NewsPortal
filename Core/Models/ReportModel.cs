using Core.Enums;
using System;
using System.Collections.Generic;

namespace Portal.ViewModels
{
    public class ReportModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public MainElement MainElement { get; set; }
        public Display Display { get; set; }
        public List<ReportItemModel> Result { get; set; } = new List<ReportItemModel>();
    }
}
