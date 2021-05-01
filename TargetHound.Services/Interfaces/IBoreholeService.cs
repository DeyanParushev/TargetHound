namespace TargetHound.Services
{
    using System.Threading.Tasks;
    
    using TargetHound.DataModels;

    public interface IBoreholeService
    {
        Task Create(Borehole borehole);
        public Task CreateBoreholeCsv(string projectId, string userId, Borehole borehole, string saveDirectory);
        Task Edit(Borehole inputBorehole);
        public Task<T> GetBorehole<T>(string boreholeId);
        
        public Task<string> GetBoreholeName(string boreholeId);
        
        public Task UpdateBoreholesAsync(string projectId, string userId, Borehole boreholes);
    }
}
