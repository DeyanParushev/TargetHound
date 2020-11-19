namespace TargetHound.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProjectService
    {
        public Task CreateAsync(string userId, string projectName, double magneticDeclination, int countryId);
       
        public Task<string> GetProjectAdminName(string projectId);
        
        public Task<T> GetProjectById<T>(string projectId);
        
        public Task<ICollection<T>> GetProjectsByUserId<T>(string userId);

        public Task<bool> IsUserIdSameWithProjectAdminId(string userId, string projectId);
    }
}
