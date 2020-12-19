namespace SendgridEmailInAspNetCore.Services
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using SendGrid;
    using SendGrid.Helpers.Errors.Model;
    using SendGrid.Helpers.Mail;

    using TargetHound.Services.Messages;

    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridClient client;

        public SendGridEmailSender()
        {
            string apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            this.client = new SendGridClient(apiKey);
        }

        public async Task SendEmailAsync(
            string sender,
            string senderName,
            string receiver,
            string receiverName,
            string subject,
            string htmlContent)
        {
            if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(htmlContent))
            {
                throw new ArgumentException("Subject and message should be provided.");
            }

            var from = new EmailAddress(sender, senderName);
            var to = new EmailAddress(receiver, receiverName);
            var plainTextContent = htmlContent;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await this.client.SendEmailAsync(msg);

            if (!response.StatusCode.Equals(HttpStatusCode.Accepted))
            {
                throw new ForbiddenException(
                    "Something went wrong, we couldn`t send the email. We will notify the site admin.");
            }
        }
    }
}