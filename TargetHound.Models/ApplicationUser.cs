namespace TargetHound.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            base.Id = Guid.NewGuid().ToString();
            this.UserProjects = new HashSet<UserProject>();
        }

        public ICollection<UserProject> UserProjects { get; set; }

        [ForeignKey("Client")]
        public string ClientId { get; set; }

        public virtual Client Client { get; set; }

        public bool IsDeleted { get; set; }
    }
}
