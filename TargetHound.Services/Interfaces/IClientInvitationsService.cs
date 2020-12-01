namespace TargetHound.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IClientInvitationsService
    {
        public Task AcceptInvitationAsync(string clientId, string invitationEmail);
        
        public Task AddClientInvitationAssync(string clientId, string invitationEmail);
    }
}
