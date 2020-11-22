namespace TargetHound.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IClientService
    {
        public Task CreateClientAsync(string companyName, string userId);
        
        public Task<string> GetAdminId(string clientId);
        
        public Task<ICollection<T>> GetClientInfoByAdminId<T>(string adminId);
        
        public Task<ICollection<T>> GetClientInfoByUserId<T>(string adminId);
    }
}
