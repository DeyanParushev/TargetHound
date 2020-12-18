using System.Collections.Generic;
using System.Threading.Tasks;
using TargetHound.DataModels;

namespace TargetHound.Services
{
    public interface IBoreholeService
    {
        public Task UpdateBoreholes(string projectId, string userId, Borehole boreholes);
    }
}
