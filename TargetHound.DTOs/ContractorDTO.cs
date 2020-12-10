namespace TargetHound.DTOs
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;

    public class ContractorDTO : IMapFrom<Contractor>
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Name must be between 3 and 40 characters.")]
        [MinLength(3, ErrorMessage = "Name must be between 3 and 40 characters.")]
        [MaxLength(40, ErrorMessage = "Name must be between 3 and 40 characters.")]
        public string Name { get; set; }

        public ICollection<DrillRigDTO> Machines { get; set; }

        public bool IsActive { get; set; }
    }
}
