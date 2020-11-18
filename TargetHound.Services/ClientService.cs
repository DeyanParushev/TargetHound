namespace TargetHound.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using TargetHound.Data;
    using TargetHound.Models;
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
    }
}
