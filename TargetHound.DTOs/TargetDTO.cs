namespace TargetHound.DTOs
{
    using System.ComponentModel.DataAnnotations;
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;

    public class TargetDTO : IPoint, IMapFrom<Target>
    {
        public string Id { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(40)]
        public string Name { get; set; }
        
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