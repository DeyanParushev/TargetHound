namespace TargetHound.SharedViewModels.InputModels
{
    using System.ComponentModel.DataAnnotations;

    public class ProjectInputModel
    {
        [MinLength(5, ErrorMessage = "Project name should be between 5 and 40 characters long.")]
        [MaxLength(40, ErrorMessage = "Project name should be between 5 and 40 characters long.")]
        [Required(ErrorMessage = "Please enter a project name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Magnetic Declination is required.")]
        [Display(Name = "Magnetic Declination")]
        public double MagneticDeclination { get; set; }

        [Range(1, 220)]
        public int CountryId { get; set; }
    }
}
