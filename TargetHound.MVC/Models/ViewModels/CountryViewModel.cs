namespace TargetHound.MVC.Models.ViewModels
{
    using TargetHound.Models;
    using TargetHound.Services.Automapper;

    public class CountryViewModel : IMapFrom<Country>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
