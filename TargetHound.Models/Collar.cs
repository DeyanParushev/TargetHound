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
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public ICollection<Borehole> Boreholes { get; set; }

        [ForeignKey("Project")]
        public string ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public bool IsDeleted { get; set; }

        public double Depth { get; set; }

        public double Azimuth { get; set; }

        public double Dip { get; set; }

        public double Easting { get; set; }

        public double Northing { get; set; }

        public double Elevation { get; set; }
    }
}
