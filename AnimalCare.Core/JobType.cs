using AnimalCare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalCare.Core
{
    [Serializable]
    public class Job
    {
        public Animal Animal { get; set; }
        public JobEnum JobType { get; set; }
        public int Cost { get; set; }
        public DateTime JobDate { get; set; }

        public Job()
        {
            JobDate = DateTime.Now; // стандартне значення
        }

        public Job(Animal animal, JobEnum jobType, int cost, DateTime? jobDate = null)
        {
            Animal = animal;
            JobType = jobType;
            Cost = cost;
            JobDate = jobDate ?? DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Animal} – {JobType}, {Cost} UAH on {JobDate:dd.MM.yyyy}";
        }
    }
}