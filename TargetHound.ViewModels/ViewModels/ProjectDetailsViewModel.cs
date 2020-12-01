namespace TargetHound.SharedViewModels.ViewModels
{
    using System.Collections.Generic;
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;

    public class ProjectDetailsViewModel : IMapFrom<Project>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int UserCount { get; set; }

        public string CurrentContractorName { get; set; }

        public ICollection<DrillRigViewModel> Machines { get; set; }

        public ICollection<BoreholeViewModel> Boreholes { get; set; }

        public ICollection<CollarViewModel> Collars { get; set; }

        public ICollection<TargetViewModel> Targets { get; set; }
    }
}
