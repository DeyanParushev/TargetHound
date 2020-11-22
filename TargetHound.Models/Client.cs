namespace TargetHound.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Client
    {
        public Client()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Users = new HashSet<ApplicationUser>();
            this.Projects = new HashSet<Project>();
            this.ClientContractors = new HashSet<ClientContractor>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [ForeignKey("ApplicationUser")]
        public string AdminId { get; set; }

        public ApplicationUser Admin { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<ClientContractor> ClientContractors { get; set; }

        public bool IsDeleted { get; set; }
    }
}
