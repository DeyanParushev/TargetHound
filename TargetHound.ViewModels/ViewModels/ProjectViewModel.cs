namespace TargetHound.ViewModels.ViewModels
{
    using System.Collections.Generic;

    using TargetHound.Models;
    using TargetHound.MVC.Models.ViewModels;
    using TargetHound.Services.Automapper;
    using TargetHound.SharedViewModels.ViewModels;

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
