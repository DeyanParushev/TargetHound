namespace TargetHound.DTOs
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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

        [Required(ErrorMessage = "Name must be between 6 and 40 characters.")]
        [MinLength(6, ErrorMessage = "Name must be between 6 and 40 characters.")]
        [MaxLength(40, ErrorMessage = "Name must be between 6 and 40 characters.")]
        public string Name { get; set; }

        [Required]
        public double MagneticDeclination { get; set; }

        public IList<BoreholeDTO> Boreholes { get; set; }

        public IList<TargetDTO> Targets { get; set; }

        public IList<CollarDTO> Collars { get; set; }

        public IList<ContractorDTO> Contractors { get; set; }
    }
}
