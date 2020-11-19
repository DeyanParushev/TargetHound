namespace TargetHound.SharedViewModels.InputModels
{
    using System.Collections.Generic;
    using TargetHound.SharedViewModels.ViewModels;

    public class ProjectCountryInputModel
    {
        public ProjectInputModel Project { get; set; }

        public ICollection<CountryViewModel> Countries { get; set; }
    }
}
