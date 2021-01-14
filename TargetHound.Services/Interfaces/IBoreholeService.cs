namespace TargetHound.Services
{
    using System.Threading.Tasks;
    
    using TargetHound.DataModels;

    public interface IBoreholeService
    {
        public Task ExportBoreholeAsync(string projectId, string userId, Borehole borehole, string saveDirectory);
        
        public Task<string> GetBoreholeName(string boreholeId);
        
        public Task UpdateBoreholesAsync(string projectId, string userId, Borehole boreholes);
    }
}
