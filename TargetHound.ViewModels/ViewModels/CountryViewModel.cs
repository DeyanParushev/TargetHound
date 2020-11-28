namespace TargetHound.SharedViewModels.ViewModels
{
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;

    public class CountryViewModel : IMapFrom<Country>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
