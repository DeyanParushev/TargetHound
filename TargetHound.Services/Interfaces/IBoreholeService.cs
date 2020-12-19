using System.Collections.Generic;
using System.Threading.Tasks;
using TargetHound.DataModels;

namespace TargetHound.Services
{
    public interface IBoreholeService
    {
        public Task<string> ExportBoreholeAsync(string projectId, string userId, Borehole borehole);
       
        public Task UpdateBoreholesAsync(string projectId, string userId, Borehole boreholes);
    }
}
