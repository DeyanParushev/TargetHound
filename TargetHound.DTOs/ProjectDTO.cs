namespace TargetHound.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;

    public class ProjectDTO : IMapFrom<Project>, IMapTo<Project>
    {
        public ProjectDTO()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Boreholes = new List<BoreholeDTO>();
            this.Collars = new List<CollarDTO>();
            this.Targets = new List<TargetDTO>();
            this.Contractors = new List<ContractorDTO>();
        }

        public string Id { get; set; }

        [Required(ErrorMessage = "Name must be between 5 and 40 characters.")]
        [MinLength(5, ErrorMessage = "Name must be between 5 and 40 characters.")]
        [MaxLength(40, ErrorMessage = "Name must be between 5 and 40 characters.")]
        public string Name { get; set; }

        public string AdminId { get; set; }

        public int CountryId { get; set; }

        public string ClientId { get; set; }

        public string CurrentUserId { get; set; }

        [Required]
        public double MagneticDeclination { get; set; }

        public IList<BoreholeDTO> Boreholes { get; set; }

        public IList<TargetDTO> Targets { get; set; }

        public IList<CollarDTO> Collars { get; set; }

        public IList<ContractorDTO> Contractors { get; set; }
    }
}
