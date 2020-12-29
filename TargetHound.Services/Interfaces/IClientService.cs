namespace TargetHound.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IClientService
    {
        public Task<string> CreateClientAsync(string companyName, string userId);
        
        public Task<string> GetClientAdminIdAsync(string clientId);
        
        public Task<T> GetClientInfoByAdminIdAsync<T>(string clientId, string adminId);
        
        public Task<IEnumerable<T>> GetAllClientsByAdminIdAsync<T>(string adminId);
       
        public Task<bool> AsignAdminAsync(string clientId, string userId);
        
        public Task<bool> ChangeClientNameAsync(string clientId, string clientName);
        
        public Task<ICollection<T>> GetClientUsersAsync<T>(string clientId);
        
        public Task<bool> ChangeClientAdminAsync(string clientId, string newAdminId);
        
        public Task<bool> IsUserClientAdminAsync(string userId, string clientId);
        
        public Task<bool> AsignUserToClientAsync(string userId, string clientId);
        
        public Task SetClientToNullAsync(string userId, string clientId);
        
        public Task<string> GetClientNameByIdAsync(string clientId);
        
        public Task<ICollection<T>> GetInactiveClientsAsync<T>(string userId);
        
        public Task ActivateClient(string clientId, string userId);
    }
}
