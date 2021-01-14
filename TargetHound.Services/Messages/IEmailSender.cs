namespace TargetHound.Services.Messages
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEmailSender
    {
        public Task SendEmailAsync(
            string sender, 
            string senderName, 
            string receiver, 
            string receiverName,
            string subject, 
            string plaintTextContent,
            string htmlContent);
    }
}
