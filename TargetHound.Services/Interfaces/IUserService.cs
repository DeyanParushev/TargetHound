namespace TargetHound.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IUserService
    {
        public Task<string> GetClientIdByAdminId(string userId);
        
        public Task<string> GetUserNameById(string userId);
        Task SendClientInvitationAsync(string senderEmail, string senderName, string receiverEmail, string receiverName, string clientId, string linkToJoin);
        public Task SendProjectInvitationAsync(string senderEmail, string senderName, string receiverEmail, string receiverName, string projectId, string linkToJoin);
    }
}
