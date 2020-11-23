namespace TargetHound.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TargetHound.Data;
    using TargetHound.Models;
    using TargetHound.Services.Automapper;
    using TargetHound.Services.Interfaces;

    public class ClientService : IClientService
    {
        private readonly TargetHoundContext dbContext;

        public ClientService(TargetHoundContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<string> CreateClientAsync(string companyName, string userId)
        {
            var client = new Client
            {
                Name = companyName,
                AdminId = userId,
            };

            var user = this.dbContext.Users.SingleOrDefault(x => x.Id == userId);
            client.Users.Add(user);
            user.ClientId = client.Id;
            await this.dbContext.Clients.AddAsync(client);
            await this.dbContext.SaveChangesAsync();

            return client.Id;
        }

        public async Task<string> GetAdminId(string clientId)
        {
            string adminId = this.dbContext.
                Clients.SingleOrDefault(x => x.Id == clientId)?.AdminId;

            return adminId;
        }

        public async Task<ICollection<T>> GetAllClientsByAdminId<T>(string userId)
        {
            var clientInfo = this.dbContext.Clients
                .Where(x => x.AdminId == userId && x.IsDeleted == false)
                .To<T>()
                .ToList();

            return clientInfo;
        }

        public async Task<bool> AsingAdminAsync(string clientId, string userId)
        {
            var client = this.dbContext.Clients.SingleOrDefault(x => x.Id == clientId);
            if (!this.dbContext.ApplicationUsers.Any(x => x.Id == userId))
            {
                return false;
            }

            client.AdminId = userId;
            var user = this.dbContext.ApplicationUsers.SingleOrDefault(x => x.Id == userId);
            user.ClientId = client.Id;
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<T> GetClientInfoByAdminId<T>(string clientId, string adminId)
        {
            var clientInfo = this.dbContext.Clients
                .Where(x => x.Id == clientId && x.AdminId == adminId)
                .To<T>()
                .FirstOrDefault();

            return clientInfo;
        }

        public async Task<bool> ChangeClientNameAsync(string clientId, string clientName)
        {
            var client = this.dbContext.Clients.SingleOrDefault(x => x.Id == clientId);

            if(client == null)
            {
                return false;
            }

            client.Name = clientName;
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<ICollection<T>> GetClientUsersAsync<T>(string clientId)
        {
            var clientUsers = this.dbContext.ApplicationUsers
                .Where(x => x.ClientId == clientId && x.IsDeleted == false)
                .To<T>()
                .ToList();

            return clientUsers;
        }

        public async Task<bool> ChangeClientAdmin(string clientId, string newAdminId)
        {
            if(!this.dbContext.Clients.Any(x => x.Id == clientId && x.IsDeleted == false))
            {
                return false;
            }

            if(!this.dbContext.ApplicationUsers.Any(x => x.Id == newAdminId))
            {
                return false;
            }

            this.dbContext.Clients.SingleOrDefault(x => x.Id == clientId).AdminId = newAdminId;
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsUserClientAdmin(string userId, string clientId)
        {
            if(!this.dbContext.ApplicationUsers.Any(x => x.Id == userId))
            {
                return false;
            }

            if (!this.dbContext.Clients.Any(x => x.Id == clientId))
            {
                return false;
            }

            return this.dbContext.Clients.SingleOrDefault(x => x.Id == clientId).AdminId == userId;
        }

        public async Task<bool> AsignUserToClient(string userId, string clientId)
        {
            if(this.dbContext.ApplicationUsers.Any(x => x.Id == userId))
            {
                return false;
            }

            if(this.dbContext.Clients.Any(x => x.Id == clientId))
            {
                return false;
            }

            var user = this.dbContext.ApplicationUsers.SingleOrDefault(x => x.Id == userId);
            user.ClientId = clientId;
            await this.dbContext.SaveChangesAsync();
            
            return true;
        }
    }
}
