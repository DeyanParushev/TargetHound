namespace TargetHound.DTOs
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;

    public class TargetDTO : IPoint, IMapFrom<Target>, IMapTo<Target>, IEquatable<TargetDTO>
    {
        public TargetDTO()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string ProjectId { get; set; }

        public string BoreholeId { get; set; }

        [Required(ErrorMessage = "Name must be betweem 4 and 40 characters.")]
        [MinLength(4, ErrorMessage = "Name must be betweem 4 and 40 characters.")]
        [MaxLength(40, ErrorMessage = "Name must be betweem 4 and 40 characters.")]
        public string Name { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "Depth must be a possitive number.")]
        public double Depth { get; set; }
        
        [Range(0, 360, ErrorMessage = "Azimuith must be between 0 and 360 degrees.")]
        public double Azimuth { get; set; }
        
        [Range(-90, 90, ErrorMessage = "Dip must be between -90 and 90 degrees.")]
        public double Dip { get; set; }
        
        [Required]
        public double Easting { get; set; }
        
        [Required]
        public double Northing { get; set; }
        
        [Required]
        public double Elevation { get; set; }

        public bool Equals(TargetDTO other)
        {
            if (this.Northing == other.Northing && this.Easting == other.Easting && this.Northing == other.Northing)
            {
                return true;
            }

            return false;
        }
    }
}