using AnimalCare.Core;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace AnimalCare
{
    public class CareEditorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int originalJobCount;
        private int editingJobIndex = -1;

        private void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        public Care EditedCare { get; set; }

        public ObservableCollection<Job> CareJobs => EditedCare.CareJobs;

        public bool IsSaved { get; private set; } = false;
        public bool IsEditing => editingJobIndex >= 0;

        private string ownerSurname;
        public string OwnerSurname
        {
            get => ownerSurname;
            set
            {
                if (ownerSurname != value)
                {
                    ownerSurname = value;
                    OnPropertyChanged(nameof(OwnerSurname));
                }
            }
        }

        private string species;
        public string Species
        {
            get => species;
            set
            {
                if (species != value)
                {
                    species = value;
                    OnPropertyChanged(nameof(Species));
                }
            }
        }

        private string animalName;
        public string AnimalName
        {
            get => animalName;
            set
            {
                if (animalName != value)
                {
                    animalName = value;
                    OnPropertyChanged(nameof(AnimalName));
                }
            }
        }

        private string year;
        public string Year
        {
            get => year;
            set
            {
                if (year != value)
                {
                    year = value;
                    OnPropertyChanged(nameof(Year));
                }
            }
        }

        private string cost;
        public string Cost
        {
            get => cost;
            set
            {
                if (cost != value)
                {
                    cost = value;
                    OnPropertyChanged(nameof(Cost));
                }
            }
        }

        private JobEnum? selectedJobType;
        public JobEnum? SelectedJobType
        {
            get => selectedJobType;
            set
            {
                if (selectedJobType != value)
                {
                    selectedJobType = value;
                    OnPropertyChanged(nameof(SelectedJobType));
                }
            }
        }

        private bool isMale = true;
        public bool IsMale
        {
            get => isMale;
            set
            {
                if (isMale != value)
                {
                    isMale = value;
                    OnPropertyChanged(nameof(IsMale));
                }
            }
        }

        private DateTime jobDate = DateTime.Today;
        public DateTime JobDate
        {
            get => jobDate;
            set
            {
                if (jobDate != value)
                {
                    jobDate = value;
                    OnPropertyChanged(nameof(JobDate));
                }
            }
        }

        public ObservableCollection<JobEnum> JobTypes { get; } =
            new ObservableCollection<JobEnum>(Enum.GetValues(typeof(JobEnum)).Cast<JobEnum>());

        public ObservableCollection<string> SpeciesList { get; } = new ObservableCollection<string>();

        public CareEditorViewModel(Care existingCare)
        {
            EditedCare = existingCare;
            OwnerSurname = existingCare.OwnerSurname;
            SaveOriginalSnapshot();
            LoadSpeciesList();
        }

        private void LoadSpeciesList()
        {
            string speciesPath = @"C:\Users\marym\OneDrive\Робочий стіл\WpfApp1\WpfApp1\Resourses\Species.txt";
            if (System.IO.File.Exists(speciesPath))
            {
                var species = System.IO.File.ReadAllLines(speciesPath);
                foreach (var s in species)
                    SpeciesList.Add(s);
            }
        }

        public bool ValidateJobInput(out string errorMessage)
        {
            errorMessage = "";

            if (string.IsNullOrWhiteSpace(Species) ||
                string.IsNullOrWhiteSpace(AnimalName) ||
                !int.TryParse(Year, out int birthYear) ||
                !int.TryParse(Cost, out int cost) ||
                SelectedJobType == null)
            {
                errorMessage = "Please fill in all fields correctly.";
                return false;
            }

            int currentYear = DateTime.Today.Year;
            if (birthYear > currentYear || birthYear < currentYear - 30)
            {
                errorMessage = $"The animal's birth year must be between {currentYear - 30} and {currentYear}.";
                return false;
            }

            if (JobDate.Date < DateTime.Today)
            {
                errorMessage = "The work date cannot be earlier than today.";
                return false;
            }

            if (cost < 100 || cost > 25000)
            {
                errorMessage = "The cost should be between 100 and 25,000.";
                return false;
            }

            return true;
        }

        public bool AddOrUpdateJob(out string errorMessage)
        {
            if (!ValidateJobInput(out errorMessage))
                return false;

            var animal = new Animal(Species, AnimalName, int.Parse(Year), !IsMale);
            var job = new Job(animal, SelectedJobType.Value, int.Parse(Cost), JobDate);

            bool duplicate = EditedCare.CareJobs.Any(j =>
                j.Animal.Species == job.Animal.Species &&
                j.Animal.Name == job.Animal.Name &&
                j.Animal.BirthYear == job.Animal.BirthYear &&
                j.Animal.IsMale == job.Animal.IsMale &&
                j.JobType == job.JobType &&
                j.Cost == job.Cost &&
                j.JobDate.Date == job.JobDate.Date);

            if (!IsEditing && duplicate)
            {
                errorMessage = "Such work already exists.";
                return false;
            }

            if (IsEditing)
            {
                EditedCare.CareJobs[editingJobIndex] = job;
                editingJobIndex = -1;
                OnPropertyChanged(nameof(IsEditing));
            }
            else
            {
                EditedCare.AddJob(job);
            }

            SaveOriginalSnapshot();
            ClearJobInputs();

            errorMessage = "";
            return true;
        }

        public void CancelEdit()
        {
            ClearJobInputs();
            editingJobIndex = -1;
            OnPropertyChanged(nameof(IsEditing));
        }

        public void ClearJobInputs()
        {
            Species = "";
            AnimalName = "";
            Year = "";
            Cost = "";
            SelectedJobType = null;
            IsMale = true;
            JobDate = DateTime.Today;
            editingJobIndex = -1;
            OnPropertyChanged(nameof(IsEditing));
        }

        private void SaveOriginalSnapshot()
        {
            originalJobCount = EditedCare.CareJobs.Count;
        }

        public bool IsUnsavedJobEntryPresent()
        {
            return EditedCare.CareJobs.Count < originalJobCount;
        }

        public void SaveChanges()
        {
            EditedCare.OwnerSurname = OwnerSurname;
            IsSaved = true;
        }

        public void StartEditJob(int index)
        {
            if (index < 0 || index >= EditedCare.CareJobs.Count)
                return;

            var job = EditedCare.CareJobs[index];
            Species = job.Animal.Species;
            AnimalName = job.Animal.Name;
            Year = job.Animal.BirthYear.ToString();
            Cost = job.Cost.ToString();
            SelectedJobType = job.JobType;
            IsMale = !job.Animal.IsMale;
            JobDate = job.JobDate;

            editingJobIndex = index;
            OnPropertyChanged(nameof(IsEditing));
        }
    }
}
