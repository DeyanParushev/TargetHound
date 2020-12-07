namespace TargetHound.DTOs
{
    using System.ComponentModel.DataAnnotations;
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;

    public class CollarDTO : IPoint, IMapFrom<Collar>
    {
        public string Id { get; set; }

        public double Depth { get; set; }
       
        public double Azimuth { get; set; }
        
        public double Dip { get; set; }
        
        [Required]
        public double Easting { get; set; }
        
        [Required]
        public double Northing { get; set; }
        
        [Required]
        public double Elevation { get; set; }
    }
}
