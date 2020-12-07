namespace TargetHound.DTOs
{
    using System.Collections.Generic;
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;

    public class ProjectDTO : IMapFrom<Project>
    {
        public ProjectDTO()
        {
            this.Boreholes = new List<BoreholeDTO>();
            this.Collars = new List<CollarDTO>();
            this.Targets = new List<TargetDTO>();
            this.Contractors = new List<ContractorDTO>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public double MagneticDeclination { get; set; }

        public string ContractorName { get; set; }

        public IList<BoreholeDTO> Boreholes { get; set; }

        public IList<TargetDTO> Targets { get; set; }

        public IList<CollarDTO> Collars { get; set; }

        public IList<ContractorDTO> Contractors { get; set; }
    }
}
