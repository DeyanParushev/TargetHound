namespace TargetHound.Services
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using TargetHound.Data;
    using TargetHound.Services.Interfaces;
    using TargetHound.Services.Messages;

    public class UserService : IUserService
    {
        private readonly TargetHoundContext dbContext;
        private readonly IEmailSender emailSender;

        public UserService(TargetHoundContext dbContext, IEmailSender emailSender)
        {
            this.dbContext = dbContext;
            this.emailSender = emailSender;
        }

        public async Task<string> GetUserNameById(string userId)
        {
            string userName = this.dbContext.ApplicationUsers.SingleOrDefault(x => x.Id == userId)?.UserName;
            return userName;
        }

        public async Task<string> GetClientIdByAdminId(string userId)
        {
            string clientId = this.dbContext.Clients.SingleOrDefault(x => x.AdminId == userId)?.Id;
            return clientId;
        }

        public async Task SendProjectInvitationAsync(string senderEmail, string senderName, string receiverEmail, string receiverName, string projectId, string linkToJoin)
        {
            var projectName = this.dbContext
                .Projects
                .SingleOrDefault(x => x.Id == projectId && x.IsDeleted == false).Name;
            
            var projectInvitationMessage = 
                MessageTemplates.ProjectInvitation(senderName, receiverName, projectName, linkToJoin);
            
            await this.emailSender
                .SendEmailAsync(
                senderEmail, 
                senderName, 
                receiverEmail, 
                receiverName, 
                MessageTemplates.InvitationSubject(senderName), 
                projectInvitationMessage);
        }

        public async Task SendClientInvitationAsync(string senderEmail, string senderName, string receiverEmail, string receiverName, string clientId, string linkToJoin)
        {
            var clientName = this.dbContext
                .Clients
                .SingleOrDefault(x => x.Id == clientId && x.IsDeleted == false).Name;

            var clientInvitationMessage = 
                MessageTemplates.ClientInvitation(senderName, clientName, linkToJoin);

            await this.emailSender
                .SendEmailAsync(
                senderEmail, 
                senderName, 
                receiverEmail,
                receiverName, 
                MessageTemplates.InvitationSubject(senderName), 
                clientInvitationMessage);
        }
    }
}
