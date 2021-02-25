namespace SendgridEmailInAspNetCore.Services
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    using SendGrid;
    using SendGrid.Helpers.Errors.Model;
    using SendGrid.Helpers.Mail;
    using TargetHound.Services.Interfaces;
    using TargetHound.Services.Messages;

    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridClient client;
        private readonly string apiKey;

        public SendGridEmailSender(ISecretsService secretsService)
        {
            this.apiKey = secretsService.GetSecret("SendGrid", "ApiKey");
            this.client = new SendGridClient(this.apiKey);
        }

        public async Task SendEmailAsync(
            string sender,
            string senderName,
            string receiver,
            string receiverName,
            string subject,
            string plainTextContent,
            string htmlContent)
        {
            if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(htmlContent))
            {
                throw new ArgumentException("Subject and message should be provided.");
            }

            var from = new EmailAddress(sender, senderName);
            var to = new EmailAddress(receiver, receiverName);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await this.client.SendEmailAsync(msg);
            
            if (!response.StatusCode.Equals(HttpStatusCode.Accepted))
            {
                throw new ForbiddenException(
                    "Something went wrong, we couldn`t send the email. We will notify the site admin.");
            }
        }

        //public void MailMessageHtml(string body, string subject, string from, IEnumerable<string> to)
        //{
        //    var message = new MailMessage();
        //    message.To.Add(new MailAddress("myemail@example.com"));
        //    message.From = new MailAddress(_Settings.MailServer.UserName);
        //    message.Subject = subject;
        //    message.IsBodyHtml = true;

        //    var htmlBody = AlternateView.CreateAlternateViewFromString(
        //        body, Encoding.UTF8, "text/html");

        //    message.AlternateViews.Add(
        //        AlternateView.CreateAlternateViewFromString(string.Empty, new ContentType("text/plain")));

        //    message.AlternateViews.Add(htmlBody);

        //    using (var smtp = new SmtpClient())
        //    {
        //        var credential = new NetworkCredential
        //        {
        //            UserName = _Settings.MailServer.UserName,
        //            Password = _Settings.MailServer.Password
        //        };
        //        smtp.Credentials = credential;
        //        smtp.Host = _Settings.MailServer.IPAddress;
        //        smtp.Port = _Settings.MailServer.Port;
        //        smtp.EnableSsl = _Settings.MailServer.EnableSSL;
        //        smtp.Send(message);
        //    }
        //}
    }
}