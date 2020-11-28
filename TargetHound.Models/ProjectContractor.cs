namespace TargetHound.DataModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ProjectContractor
    {
        public ProjectContractor()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [ForeignKey("Project")]
        public string ProjectId { get; set; }

        public virtual Project Project { get; set; }

        [ForeignKey("Contractor")]
        public string ContractorId { get; set; }

        public virtual Contractor Contractor { get; set; }

        public bool IsDeleted { get; set; }
    }
}
