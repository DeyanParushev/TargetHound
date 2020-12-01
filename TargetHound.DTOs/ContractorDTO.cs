namespace TargetHound.DTOs
{
    using System.Collections.Generic;

    public class ContractorDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<DrillRigDTO> Machines { get; set; }

        public bool IsActive { get; set; }
    }
}
