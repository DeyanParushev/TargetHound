namespace TargetHound.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IClientService
    {
        public Task<string> CreateClientAsync(string companyName, string userId);
        
        public Task<string> GetAdminId(string clientId);
        
        public Task<T> GetClientInfoByAdminId<T>(string clientId, string adminId);
        
        public Task<ICollection<T>> GetAllClientsByAdminId<T>(string adminId);
       
        public Task<bool> AsingAdmin(string clientId, string userId);
    }
}
