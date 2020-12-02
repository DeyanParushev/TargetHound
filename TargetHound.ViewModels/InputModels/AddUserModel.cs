namespace TargetHound.SharedViewModels.InputModels
{
    using System.ComponentModel.DataAnnotations;

    public class AddUserModel
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email adress.")]
        public string Email { get; set; }
    }
}
