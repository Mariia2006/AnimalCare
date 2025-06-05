using AnimalCare.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalCare
{
    public class ReportViewModel
    {
        public string OwnerSurname { get; set; }
        public ObservableCollection<Job> CareJobs { get; set; }

        public int TotalCost => CareJobs.Sum(j => j.Cost);

        public ReportViewModel(Care care)
        {
            OwnerSurname = care.OwnerSurname;
            CareJobs = care.CareJobs;
        }
    }
}
