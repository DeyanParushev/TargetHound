namespace TargetHound.DataModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using TargetHound.DataModels.Interfaces;

    public class Target : IPoint
    {
        public Target()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [ForeignKey("Borehole")]
        public string BoreholeId { get; set; }

        public virtual Borehole Borehole { get; set; }

        public bool IsDeleted { get; set; }

        [ForeignKey("Project")]
        public string ProjectId { get; set; }

        public virtual Project Project { get; set; }
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
