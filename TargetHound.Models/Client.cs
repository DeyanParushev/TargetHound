namespace TargetHound.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Client
    {
        public Client()
        {
            this.ClientId = Guid.NewGuid().ToString();
            this.Users = new HashSet<ApplicationUser>();
            this.Projects = new HashSet<Project>();
            this.ClientContractors = new HashSet<ClientContractor>();
        }

        [Key]
        public string ClientId { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<ClientContractor> ClientContractors { get; set; }

        public bool IsDeleted { get; set; }
    }
}
