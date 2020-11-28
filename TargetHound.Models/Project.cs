namespace TargetHound.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Project
    {
        public Project()
        {
            this.ProjectContractors = new HashSet<ProjectContractor>();
            this.Boreholes = new HashSet<Borehole>();
            this.Collars = new HashSet<Collar>();
            this.Targets = new HashSet<Target>();
            this.ProjectUsers = new HashSet<UserProject>();
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        public double MagneticDeclination { get; set; }

        [ForeignKey("Country")]
        public int? CountryId { get; set; }

        public virtual Country Country { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual ICollection<ProjectContractor> ProjectContractors { get; set; }

        public virtual ICollection<Borehole> Boreholes { get; set; }

        public virtual ICollection<Collar> Collars { get; set; }

        public virtual ICollection<Target> Targets { get; set; }

        public virtual ICollection<UserProject> ProjectUsers { get; set; }

        [ForeignKey("Client")]
        public string ClientId { get; set; }

        public virtual Client Client { get; set; }

        [ForeignKey("ApplicationUser")]
        public string AdminId { get; set; }

        public ApplicationUser Admin { get; set; }

        public bool IsDeleted { get; set; }
    }
}
