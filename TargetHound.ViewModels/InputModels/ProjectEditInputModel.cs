namespace TargetHound.SharedViewModels.InputModels
{
    using System.ComponentModel.DataAnnotations;
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;

    public class ProjectEditInputModel : IMapFrom<Project>
    {
        public string Id { get; set; }

        [MinLength(5, ErrorMessage = "Project name should be between 5 and 40 characters long.")]
        [MaxLength(40, ErrorMessage = "Project name should be between 5 and 40 characters long.")]
        [Required(ErrorMessage = "Please enter a project name.")]
        public string Name { get; set; }

        public string CountryName { get; set; }

        public double MagneticDeclination { get; set; }

        public string AdminId { get; set; }
    }
}
