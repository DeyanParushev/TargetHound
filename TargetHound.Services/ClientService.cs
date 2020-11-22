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
            Client client = new Client
            {
                Name = companyName,
                AdminId = userId,
            };

            client.Users.Add(this.dbContext.Users.SingleOrDefault(x => x.Id == userId));
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
                .Where(x => x.AdminId == userId)
                .To<T>()
                .ToList();

            return clientInfo;
        }

        public async Task<bool> AsingAdmin(string clientId, string userId)
        {
            Client client = this.dbContext.Clients.SingleOrDefault(x => x.Id == clientId);
            if (!this.dbContext.ApplicationUsers.Any(x => x.Id == userId))
            {
                return false;
            }

            client.AdminId = userId;
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
    }
}
