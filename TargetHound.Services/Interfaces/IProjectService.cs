namespace TargetHound.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IProjectService
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task Create(string userId, string projectName, double magneticDeclination) { }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}
