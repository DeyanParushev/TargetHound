namespace TargetHound.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IProjectService
    {
        public async Task Create(string userId, string projectName, double magneticDeclination) { }
    }
}
