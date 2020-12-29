namespace TargetHound.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TargetHound.Data;
    using TargetHound.DataModels;
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

        public async Task<string> GetClientAdminIdAsync(string clientId)
        {
            string adminId = this.dbContext.Clients.SingleOrDefault(x => x.Id == clientId)?.AdminId;
            return adminId;
        }

        public async Task<IEnumerable<T>> GetAllClientsByAdminIdAsync<T>(string userId)
        {
            var clientInfo = this.dbContext.Clients
                .Where(x => x.AdminId == userId)
                .To<T>()
                .ToList();

            return clientInfo;
        }

        public async Task<bool> AsignAdminAsync(string clientId, string userId)
        {
            this.CheckIfClientExists(clientId);
            this.CheckIfUserExists(userId);

            var client = this.dbContext.Clients.SingleOrDefault(x => x.Id == clientId && x.IsDeleted == false);

            client.AdminId = userId;
            var user = this.dbContext.ApplicationUsers.SingleOrDefault(x => x.Id == userId && x.IsDeleted == false);
            user.ClientId = client.Id;
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<T> GetClientInfoByAdminIdAsync<T>(string clientId, string adminId)
        {
            var clientInfo = this.dbContext.Clients
                .Where(x => x.Id == clientId && x.AdminId == adminId)
                .To<T>()
                .FirstOrDefault();

            return clientInfo;
        }

        public async Task ActivateClient(string clientId, string userId)
        {
            var client = this.dbContext.Clients.FirstOrDefault(x => x.IsDeleted == true && x.Id == clientId);
            client.IsDeleted = true;

            await this.dbContext.SaveChangesAsync();
        }

        public async Task<bool> ChangeClientNameAsync(string clientId, string clientName)
        {
            var client = this.dbContext.Clients.SingleOrDefault(x => x.Id == clientId && x.IsDeleted == false);

            if (client == null)
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

        public async Task<bool> ChangeClientAdminAsync(string clientId, string newAdminId)
        {
            this.CheckIfClientExists(clientId);
            this.CheckIfUserExists(newAdminId);

            this.dbContext.Clients
                .SingleOrDefault(x => x.Id == clientId && x.IsDeleted == false)
                .AdminId = newAdminId;
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsUserClientAdminAsync(string userId, string clientId)
        {
            this.CheckIfClientExists(clientId);
            this.CheckIfUserExists(userId);

            return this.dbContext.Clients
                .SingleOrDefault(x => x.Id == clientId && x.IsDeleted == false)
                .AdminId == userId;
        }

        public async Task<bool> AsignUserToClientAsync(string userId, string clientId)
        {
            this.CheckIfUserExists(userId);
            this.CheckIfClientExists(clientId);

            var user = this.dbContext.ApplicationUsers.SingleOrDefault(x => x.Id == userId);
            user.ClientId = clientId;
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task SetClientToNullAsync(string userId, string clientId)
        {
            this.CheckIfUserExists(userId);
            this.CheckIfClientExists(clientId);
            var isUserAnAdmin = await this.IsUserClientAdminAsync(userId, clientId);

            if (!isUserAnAdmin)
            {
                throw new ApplicationException("You are not admin for this client.");
            }

            this.dbContext.Clients.SingleOrDefault(x => x.Id == clientId).IsDeleted = true;
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<string> GetClientNameByIdAsync(string clientId)
        {
            this.CheckIfClientExists(clientId);
            return this.dbContext.Clients.SingleOrDefault(x => x.Id == clientId).Name;
        }

        public async Task<ICollection<T>> GetInactiveClientsAsync<T>(string userId)
        {
            this.CheckIfUserExists(userId);

            var clients = this.dbContext
                .Clients
                .Where(x => x.AdminId == userId && x.IsDeleted == true)
                .To<T>()
                .ToList();

            return clients;
        }

        private void CheckIfUserExists(string userId)
        {
            if (!this.dbContext.ApplicationUsers.Any(x => x.Id == userId))
            {
                throw new ArgumentException("User doesn`t exist.");
            }
        }

        private void CheckIfClientExists(string clientId)
        {
            if (!this.dbContext.Clients.Any(x => x.Id == clientId))
            {
                throw new ArgumentException("Company doesn`t exist.");
            }
        }
    }
}
