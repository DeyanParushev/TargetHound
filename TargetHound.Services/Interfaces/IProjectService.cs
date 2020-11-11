namespace TargetHound.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TargetHound.Models;

    public interface IProjectService
    {
        public async Task Create(string userId, string projectName, double magneticDeclination) { }

        public ICollection<T> GetProjectsByUserId<T>(string userId);
    }
}
