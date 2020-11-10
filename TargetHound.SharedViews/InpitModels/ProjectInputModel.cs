using System.ComponentModel.DataAnnotations;

namespace TargetHound.InputModels
{
    public class ProjectInputModel
    {
        [MinLength(5, ErrorMessage = "Project name should be between 5 and 40 characters long.")]
        [MaxLength(40, ErrorMessage = "Project name should be between 5 and 40 characters long.")]
        [Required(ErrorMessage = "Please enter a project name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The magnetic declination is required.")]
        public double MagneticDeclination { get; set; }
    }
}
