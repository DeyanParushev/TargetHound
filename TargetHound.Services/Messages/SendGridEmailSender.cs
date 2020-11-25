namespace SendgridEmailInAspNetCore.Services
{
    using Microsoft.Extensions.Configuration;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System;
    using System.Threading.Tasks;
    using TargetHound.Services.Messages;

    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridClient client;

        public SendGridEmailSender()
        {
            string apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            this.client = new SendGridClient(apiKey);
        }

        public async Task SendEmailAsync(string sender, string fromName, string receiver, string subject, string htmlContent)
        {
            if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(htmlContent))
            {
                throw new ArgumentException("Subject and message should be provided.");
            }

            var fromAddress = new EmailAddress(sender, fromName);
            var toAddress = new EmailAddress(receiver);
            var message = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, htmlContent, htmlContent);
            await this.client.SendEmailAsync(message);
        }
    }
}

