namespace TargetHound.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TargetHound.Data;
    using TargetHound.Services.Automapper;
    using TargetHound.Services.Interfaces;

    public class ContractorService : IContractorService
    {
        private const string contractorError = "Contractor does not exist!";
        private readonly TargetHoundContext dbContext;

        public ContractorService(TargetHoundContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<T>> GetDrillRigsAsync<T>(string contractorId)
        {
            var machines = this.dbContext
                .DrillRigs
                .Where(x => x.Id == contractorId)
                .To<T>()
                .ToList();

            return machines;
        }

        private void CheckContractorExists(string contractorId)
        {
            if (!this.dbContext.Contractors.Any(x => x.Id == contractorId && x.IsDeleted == false))
            {
                throw new NullReferenceException(contractorError);
            }
        }
    }
}
