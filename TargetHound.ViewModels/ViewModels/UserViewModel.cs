namespace TargetHound.SharedViewModels.ViewModels
{
    using TargetHound.Models;
    using TargetHound.Services.Automapper;

    public class UserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string  Email { get; set; }
    }
}
