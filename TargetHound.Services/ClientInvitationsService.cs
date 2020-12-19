namespace TargetHound.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using TargetHound.Data;
    using TargetHound.DataModels;
    using TargetHound.Services.Interfaces;

    public class ClientInvitationsService : IClientInvitationsService
    {
        private readonly TargetHoundContext dbContext;

        public ClientInvitationsService(TargetHoundContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddClientInvitationAssync(string clientId, string invitationEmail)
        {
            if (!this.dbContext.Clients.Any(x => x.Id == clientId && x.IsDeleted == false))
            {
                throw new NullReferenceException("Clietn does not exist.");
            }

            var clientInvitation = new ClientInvitation
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = clientId,
                Email = invitationEmail,
                IsAccepted = false,
            };

            this.dbContext.ClientInvitations.Add(clientInvitation);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task AcceptInvitationAsync(string clientId, string invitationEmail)
        {
            if (!this.dbContext.Clients.Any(x => x.Id == clientId && x.IsDeleted == false))
            {
                throw new NullReferenceException("Clietn does not exist.");
            }

            var invitation = this.dbContext
                .ClientInvitations
                .FirstOrDefault(x => x.ClientId == clientId && x.Email == invitationEmail && x.IsAccepted == false);
            if (invitation == null)
            {
                throw new NullReferenceException("Invalid invitation");
            }

            invitation.IsAccepted = true;
            await this.dbContext.SaveChangesAsync();
        }
    }
}
