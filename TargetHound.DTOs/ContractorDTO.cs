namespace TargetHound.DTOs
{
    using System.Collections.Generic;
    using TargetHound.DataModels;
    using TargetHound.Services.Automapper;

    public class ContractorDTO : IMapFrom<Contractor>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<DrillRigDTO> Machines { get; set; }

        public bool IsActive { get; set; }
    }
}
