namespace TargetHound.SharedViewModels.InputModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;
    using TargetHound.SharedViewModels.ViewModels;

    public class ProjectEditInputModel : IMapFrom<Project>
    {
        public string Id { get; set; }

        [MinLength(5, ErrorMessage = "Project name should be between 5 and 40 characters long.")]
        [MaxLength(40, ErrorMessage = "Project name should be between 5 and 40 characters long.")]
        [Required(ErrorMessage = "Please enter a project name.")]
        public string Name { get; set; }

        public ICollection<CountryViewModel> Countries { get; set; }

        [Required]
        [Range(1, 220)]
        public int CountryId { get; set; }

        [Required]
        public double MagneticDeclination { get; set; }
    }
}
