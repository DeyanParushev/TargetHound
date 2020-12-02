namespace TargetHound.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProjectService
    {
        public Task CreateAsync(string userId, string projectName, double magneticDeclination, int countryId);
        
        public  Task EditProjectAsync(string projectId, string projectName, double magneticDeclination, int countryId);
        
        public Task<ICollection<T>> GetBoreholesAsync<T>(string projectId);
        
        public Task<ICollection<T>> GetCollarsAsync<T>(string projectId);
        
        public Task<string> GetCurrentContractorAsync(string projectId);
       
        public Task<T> GetDetailsAsync<T>(string projectId);
        
        public Task<string> GetProjectAdminName(string projectId);
        
        public Task<T> GetProjectById<T>(string projectId);
        
        public Task<ICollection<T>> GetProjectsByUserId<T>(string userId);
        
        public Task<ICollection<T>> GetProjectUsersAsync<T>(string projectId);
       
        public Task<ICollection<T>> GetTargetsAsync<T>(string projectId);
        
        public Task<int> GetUserCountAsync(string projectId);
        
        public Task<bool> IsUserIdSameWithProjectAdminId(string userId, string projectId);
       
        public Task<bool> IsUserInProject(string userId, string projectId);
    }
}
