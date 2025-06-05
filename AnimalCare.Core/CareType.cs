using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalCare.Core
{
    [Serializable]
    public class Care
    {
        public string OwnerSurname { get; set; }
        public ObservableCollection<Job> CareJobs { get; set; } = new ObservableCollection<Job>();

        public Care() { }

        public Care(string ownerSurname)
        {
            OwnerSurname = ownerSurname;
            CareJobs = new ObservableCollection<Job>();
        }

        public string Summary => $"{OwnerSurname}, Jobs: {CareJobs.Count}";

        public void AddJob(Job job) => CareJobs.Add(job);

        public string ToShortString()
        {
            int totalCost = CareJobs.Sum(j => j.Cost);
            return $"{totalCost} UAH";
        }

        public override string ToString()
        {
            var jobsStr = string.Join("\n", CareJobs.Select(j => j.ToString()));
            return $"{OwnerSurname}:\n{jobsStr}";
        }
    }
}
