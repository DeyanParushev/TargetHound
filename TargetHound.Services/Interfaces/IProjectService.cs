namespace TargetHound.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProjectService
    {
        public Task CreateAsync(string userId, string projectName, double magneticDeclination, int countryId);

        public ICollection<T> GetProjectsByUserId<T>(string userId);
    }
}
