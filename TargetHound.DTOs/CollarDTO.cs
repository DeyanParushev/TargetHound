namespace TargetHound.DTOs
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;

    public class CollarDTO : IPoint, IMapFrom<Collar>, IMapTo<Collar>, IEquatable<CollarDTO>
    {
        public CollarDTO()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string ProjectId { get; set; }

        [Required(ErrorMessage = "Name must be bewtween 3 and 40 characters")]
        [MinLength(3, ErrorMessage = "Name must be between 3 and 40 characters.")]
        [MaxLength(40, ErrorMessage = "Name must be between 3 and 40 characters.")]
        public string Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Depth can`t be a negative number.")]
        public double Depth { get; set; }
       
        [Range(0, 360, ErrorMessage = "Azimuth must be between 0 and 360 degrees.")]
        public double Azimuth { get; set; }
        
        [Range(-90, 90, ErrorMessage = "Dip must be between -90 and 90 degrees.")]
        public double Dip { get; set; }
        
        [Required]
        public double Easting { get; set; }
        
        [Required]
        public double Northing { get; set; }
        
        [Required]
        public double Elevation { get; set; }

        public bool Equals(CollarDTO other)
        {
            if(this.Northing == other.Northing && this.Easting == other.Easting && this.Northing == other.Northing)
            {
                return true;
            }

            return false;
        }
    }
}
