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

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                           ILogger<EmailSender> logger)
        {
            Options = optionsAccessor.Value;
            _logger = logger;
        }

        public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(Options.SendGridKey))
            {
                throw new Exception("Null SendGridKey");
            }

            if(subject == "Alumni Data")
            {
                await Execute(Options.SendGridKey, subject, message, toEmail, ".\\CSVFiles\\AlumniData.csv");
            }
            else
            {
                await Execute(Options.SendGridKey, subject, message, toEmail);
            }
        }


        public async Task Execute(string apiKey, string subject, string message, string toEmail)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("m.lengen1@mail.lorainccc.edu", "Elyria Alumni Association"),
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
                From = new EmailAddress("m.lengen1@mail.lorainccc.edu", "Elyria Alumni Association"),
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
