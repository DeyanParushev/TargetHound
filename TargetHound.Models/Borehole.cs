namespace TargetHound.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Borehole 
    {
        public Borehole()
        {
            this.Id = Guid.NewGuid().ToString();
            this.SurveyPoints = new List<SurveyPoint>();
        }

        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("Contractor")]
        public string ContractorId { get; set; }

        public virtual Contractor Contractor { get; set; }

        [ForeignKey("Project")]
        public string ProjectId { get; set; }

        public virtual Project Project { get; set; }

        [ForeignKey("Collar")]
        public string CollarId { get; set; }

        public virtual Collar Collar { get; set; }

        [ForeignKey("Target")]
        public string TargetId { get; set; }

        public virtual Target Targets { get; set; }

        public virtual IList<SurveyPoint> SurveyPoints { get; set; }

        public virtual ICollection<Dogleg> Doglegs { get; set; }

        public bool IsDeleted { get; set; }
    }
}
