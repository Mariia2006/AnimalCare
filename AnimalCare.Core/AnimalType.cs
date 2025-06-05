using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalCare.Core
{
    [Serializable]
    public class Animal
    {
        public string Species { get; set; }
        public string Name { get; set; }
        public int BirthYear { get; set; }
        public bool IsMale { get; set; }
        public string GenderLetter => IsMale ? "M" : "F";

        public Animal() { }

        public Animal(string species, string name, int birthYear, bool isMale)
        {
            Species = species;
            Name = name;
            BirthYear = birthYear;
            IsMale = isMale;
        }

        public override string ToString()
        {
            return $"{Species} {Name}, {BirthYear}, {(IsMale ? "male" : "female")}";
        }
    }
}
