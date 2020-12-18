namespace TargetHound.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using TargetHound.DataModels.Interfaces;

    public class Collar : IPoint
    {
        public Collar()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Boreholes = new HashSet<Borehole>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [ForeignKey("Project")]
        public string ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public virtual ICollection<Borehole> Boreholes { get; set; }

        public bool IsDeleted { get; set; } = false;

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
