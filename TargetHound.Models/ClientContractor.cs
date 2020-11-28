namespace TargetHound.DataModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ClientContractor
    {
        public ClientContractor()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [ForeignKey("Client")]
        public string CientId { get; set; }

        public virtual Client Client { get; set; }

        [ForeignKey("Contractor")]
        public string ContractorId { get; set; }

        public virtual Contractor Contractor { get; set; }

        public bool IsDeleted { get; set; }
    }
}
