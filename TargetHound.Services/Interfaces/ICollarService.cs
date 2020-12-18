using System.Collections.Generic;
using System.Threading.Tasks;
using TargetHound.DataModels;

namespace TargetHound.Services.Interfaces
{
    public interface ICollarService
    {
        public Task UpdateCollars(string projectId, string userId, ICollection<Collar> collars);
    }
}
