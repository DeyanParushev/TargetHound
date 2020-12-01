using System.Collections.Generic;
using System.Threading.Tasks;

namespace TargetHound.Services.Interfaces
{
    public interface IContractorService
    {
        public Task<ICollection<T>> GetDrillRigsAsync<T>(string contractorId);
    }
}
