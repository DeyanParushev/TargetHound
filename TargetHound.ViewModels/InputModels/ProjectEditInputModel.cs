namespace TargetHound.SharedViewModels.InputModels
{
    using TargetHound.Models;
    using TargetHound.Services.Automapper;

    public class ProjectEditInputModel : IMapFrom<Project>
    {
        public string Id { get; set; }
        
        public string Name { get; set; }

        public string CountryName { get; set; }

        public double MagneticDeclination { get; set; }

        public string AdminId { get; set; }
    }
}
