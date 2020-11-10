namespace TargetHound.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserProject
    {
        public UserProject()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Project")]
        public string ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public bool IsAdmin { get; set; }
    }
}
