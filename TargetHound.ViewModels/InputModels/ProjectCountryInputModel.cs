namespace TargetHound.MVC.Models.InputModels
{
    using System.Collections.Generic;
    using TargetHound.InputModels;
    using TargetHound.MVC.Models.ViewModels;

    public class ProjectCountryInputModel
    {
        public ProjectInputModel Project { get; set; }

        public ICollection<CountryViewModel> Cuntries { get; set; }
    }
}
