using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimalCare.Serialization;
using AnimalCare.Core;

namespace AnimalCare
{
    public class MainWindowViewModel
    {
        public List<Care> CareList { get; set; }

        public MainWindowViewModel()
        {
            CareList = Serialization.JsonCareStorage.Load();
        }

        public void AddCare(Care care)
        {
            CareList.Add(care);
            Serialization.JsonCareStorage.Save(CareList);
        }

        public void EditCare(int index, Care care)
        {
            CareList[index] = care;
            Serialization.JsonCareStorage.Save(CareList);
        }

        public void SaveChanges()
        {
            Serialization.JsonCareStorage.Save(CareList);
        }
    }
}
