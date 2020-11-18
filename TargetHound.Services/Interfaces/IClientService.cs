namespace TargetHound.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IClientService
    {
        public Task CreateClientAsync(string companyName, string userId);
    }
}
