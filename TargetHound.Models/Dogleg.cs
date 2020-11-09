namespace TargetHound.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Dogleg
    {
        public Dogleg()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        public double DoglegAngle { get; set; }

        public double DoglegSeverity { get; set; }

        public double ToolFace { get; set; }

        public double RatioFactor { get; set; }

        [ForeignKey("Borehole")]
        public string BoreholeId { get; set; }

        public virtual Borehole Borehole { get; set; }

        public bool IsDeleted { get; set; }
    }
}
