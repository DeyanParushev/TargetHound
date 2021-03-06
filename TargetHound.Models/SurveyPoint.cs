﻿namespace TargetHound.DataModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using TargetHound.DataModels.Interfaces;

    public class SurveyPoint : IPoint
    {
        public SurveyPoint()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        [Required]
        public double Depth { get; set; }

        [Required]
        public double Azimuth { get; set; }

        [Required]
        public double Dip { get; set; }

        public double Easting { get; set; }

        public double Northing { get; set; }

        public double Elevation { get; set; }
       
        public int? MagneticField { get; set; }

        [ForeignKey("Borehole")]
        public string BoreholeId { get; set; }

        public virtual Borehole Borehole { get; set; }

        public bool IsDeleted { get; set; }
    }
}
