using System.Collections.Generic;

namespace Portal.Models
{
    public class DashboardViewModel
    {
        public int Users { get; set; }
        public int Admins { get; set; }
        public int[] Views { get; set; }
        public int Happy { get; set; }
        public int Sad { get; set; }
        public int Angry { get; set; }
        public int SavedNews { get; set; }
        public int[] News { get; set; }
        public List<string> MostViewNews { get; set; } = new List<string>();
        public List<string> MostActiveUser { get; set; } = new List<string>();

    }
}
