namespace TargetHound.SharedViewModels.ViewModels
{
    using System.Collections.Generic;

    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;

    public class ProjectViewModel : IMapFrom<Project>
    {
        public ProjectViewModel()
        {
            this.Boreholes = new HashSet<BoreholeViewModel>();
            this.Collars = new HashSet<CollarViewModel>();
            this.Targets = new HashSet<TargetViewModel>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string CountryName { get; set; }
     
        public bool IsCurrentUserAdmin { get; set; }

        public string AdminName { get; set; }

        public ICollection<BoreholeViewModel> Boreholes { get; set; }
       
        public ICollection<CollarViewModel> Collars { get; set; }
      
        public ICollection<TargetViewModel> Targets { get; set; }
    }
}
