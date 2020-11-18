namespace TargetHound.SharedViewModels.ViewModels
{
    using TargetHound.Models;
    using TargetHound.Services.Automapper;

    public class CollarViewModel : IMapFrom<Collar>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double Depth { get; set; }

        public double Azimuth { get; set; }

        public double Dip { get; set; }

        public double Easting { get; set; }

        public double Northing { get; set; }

        public double Elevation { get; set; }
    }
}
