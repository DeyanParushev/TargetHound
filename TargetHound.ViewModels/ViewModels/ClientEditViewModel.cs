namespace TargetHound.SharedViewModels.ViewModels
{
    using System.Collections.Generic;

    public class ClientEditViewModel
    {
        public string Id { get; set; }

        public ICollection<UserViewModel> Users { get; set; }
    }
}
