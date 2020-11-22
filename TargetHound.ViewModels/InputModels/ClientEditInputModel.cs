namespace TargetHound.SharedViewModels.InputModels
{
    using TargetHound.Models;
    using TargetHound.Services.Automapper;

    public class ClientEditInputModel : IMapTo<Client>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
