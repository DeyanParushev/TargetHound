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
       
        public Task<bool> AsingAdminAsync(string clientId, string userId);
        
        public Task<bool> ChangeClientNameAsync(string clientId, string clientName);
        
        public Task<ICollection<T>> GetClientUsersAsync<T>(string clientId);
        
        public Task<bool> ChangeClientAdmin(string clientId, string newAdminId);
        
        public Task<bool> IsUserClientAdmin(string userId, string clientId);
        
        public Task<bool> AsignUserToClient(string userId, string clientId);
    }
}
