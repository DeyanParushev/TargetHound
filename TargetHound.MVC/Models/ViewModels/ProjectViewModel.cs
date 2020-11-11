namespace TargetHound.ViewModels.ViewModels
{
    using System.Collections.Generic;
    using TargetHound.Models;
    using TargetHound.MVC.Models.ViewModels;
    using TargetHound.Services.Automapper;

    public class ProjectViewModel : IMapFrom<Project>
    {
        public ProjectViewModel()
        {
            //this.Boreholes = new HashSet<BoreholeViewModel>();
        }

        public string Name { get; set; }

        public string CountryName { get; set; }

        //public ICollection<BoreholeViewModel> Boreholes { get; set; }
    }
}
