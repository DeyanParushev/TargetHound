namespace TargetHound.DataModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class DrillRig
    {
        public DrillRig()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Model { get; set; }

        [Required]
        [MaxLength(20)]
        public string Number { get; set; }

        [ForeignKey("Contractor")]
        public string ContractorId { get; set; }

        public virtual Contractor Contractor { get; set; }

        public bool IsDeleted { get; set; }
    }
}
