using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Text;

namespace ElyriaAlumniAssociation.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;
        private readonly string? _sendGridKey;
        private readonly string _emailFromAddress;

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                           ILogger<EmailSender> logger)
        {
            _logger = logger;
            _sendGridKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY") ?? optionsAccessor.Value.SendGridKey;
            _emailFromAddress = Environment.GetEnvironmentVariable("EMAIL_FROM_ADDRESS")!;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(_sendGridKey))
            {
                throw new Exception("Null SendGridKey");
            }

            if(subject == "Alumni Data")
            {
                await Execute(_sendGridKey, subject, message, toEmail, ".\\CSVFiles\\AlumniData.csv");
            }
            else
            {
                await Execute(_sendGridKey, subject, message, toEmail);
            }
        }


        public async Task Execute(string apiKey, string subject, string message, string toEmail)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_emailFromAddress, "Elyria Alumni Association"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(toEmail));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);
            _logger.LogInformation(response.IsSuccessStatusCode
                                   ? $"Email to {toEmail} queued successfully!"
                                   : $"Failure Email to {toEmail}");
        }

        public async Task Execute(string apiKey, string subject, string message, string toEmail, string filePath)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_emailFromAddress, "Elyria Alumni Association"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(toEmail));
            byte[] byteData = Encoding.ASCII.GetBytes(File.ReadAllText(filePath));
            msg.Attachments = new List<SendGrid.Helpers.Mail.Attachment>
        {
            new SendGrid.Helpers.Mail.Attachment
            {
                Content = Convert.ToBase64String(byteData),
                Filename = "AlumniData.csv",
                Type = "csv",
                Disposition = "attachment"
            }
        };

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);
            _logger.LogInformation(response.IsSuccessStatusCode
                                   ? $"Email to {toEmail} queued successfully!"
                                   : $"Failure Email to {toEmail}");
        }
    }
}
