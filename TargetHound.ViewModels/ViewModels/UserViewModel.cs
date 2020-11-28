namespace TargetHound.SharedViewModels.ViewModels
{
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;

    public class UserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string  Email { get; set; }
    }
}
