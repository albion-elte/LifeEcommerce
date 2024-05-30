using LifeEcommerce.Helpers.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace LifeEcommerce.Helpers
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly IConfiguration _configuration;

        public EmailSender(ILogger<EmailSender> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {

            var sendGridConfiguration = _configuration.GetSection(nameof(SendGridConfiguration)).Get<SendGridConfiguration>();
            var apiKey = sendGridConfiguration.ApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(sendGridConfiguration.SourceEmail, sendGridConfiguration.SourceName);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
