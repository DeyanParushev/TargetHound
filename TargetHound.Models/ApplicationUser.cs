namespace TargetHound.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
            : base()
        {
            this.Id = Guid.NewGuid().ToString();
            this.UserProjects = new HashSet<UserProject>();
        }

        [ForeignKey("Client")]
        public string ClientId { get; set; }

        public virtual Client Client { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<UserProject> UserProjects { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
