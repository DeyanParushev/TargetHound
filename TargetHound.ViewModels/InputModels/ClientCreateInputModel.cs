namespace TargetHound.SharedViewModels.InputModels
{
    using System.ComponentModel.DataAnnotations;

    public class ClientCreateInputModel
    {
        [Required(ErrorMessage = "Please enter a project name.")]
        [MinLength(5, ErrorMessage = "Client name must be at least 5 characters long")]
        [MaxLength(40, ErrorMessage = "Client name must be maximum 40 characters long")]
        public string Name { get; set; }
    }
}
