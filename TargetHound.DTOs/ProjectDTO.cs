using System.Collections.Generic;

namespace TargetHound.DTOs
{
    public class ProjectDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double MagneticDeclination { get; set; }

        public string ContractorName { get; set; }

        public ICollection<BoreholeDTO> Boreholes { get; set; }

        public ICollection<TargetDTO> Targets { get; set; }

        public ICollection<CollarDTO> Collars { get; set; }

        public ICollection<ContractorDTO> Contractors { get; set; }
    }
}
