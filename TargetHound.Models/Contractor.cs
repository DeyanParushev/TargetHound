namespace TargetHound.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Contractor
    {
        public Contractor()
        {
            this.DrillRigs = new HashSet<DrillRig>();
            this.Boreholes = new HashSet<Borehole>();
            this.ClientContractors = new HashSet<ClientContractor>();
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        public virtual ICollection<DrillRig> DrillRigs { get; set; }

        public virtual ICollection<Borehole> Boreholes { get; set; }

        public virtual ICollection<ClientContractor> ClientContractors { get; set; }

        public bool IsDeleted { get; set; }
    }
}
