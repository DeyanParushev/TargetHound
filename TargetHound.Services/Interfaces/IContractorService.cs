namespace TargetHound.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IContractorService
    {
        public Task<ICollection<T>> GetDrillRigsAsync<T>(string contractorId);
    }
}
