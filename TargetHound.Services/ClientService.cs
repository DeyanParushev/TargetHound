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

        public async Task CreateClientAsync(string companyName, string userId)
        {
            Client client = new Client
            {
                Name = companyName,
            };

            client.Users.Add(this.dbContext.Users.SingleOrDefault(x => x.Id == userId));
            await this.dbContext.Clients.AddAsync(client);
        }

        public async Task<string> GetAdminId(string clientId)
        {
            string adminId = this.dbContext.
                Clients.SingleOrDefault(x => x.ClientId == clientId)?.AdminId;

            return adminId;
        }

        public async Task<ICollection<T>> GetClientInfoByUserId<T>(string userId)
        {
            var clientInfo = this.dbContext.Clients
                .Where(x => x.Users.Any(y => y.Id == userId))
                .To<T>()
                .ToList();

            return clientInfo;
        }

        public async Task<ICollection<T>> GetClientInfoByAdminId<T>(string adminId)
        {
            var clientInfo = this.dbContext.Clients
                .Where(x => x.AdminId == adminId)
                .To<T>()
                .ToList();

            return clientInfo;
        }
    }
}
