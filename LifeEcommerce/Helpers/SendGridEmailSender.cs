using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace LifeEcommerce.Helpers
{
    public class SendGridEmailSender //: IEmailSender
    {
        private readonly ILogger<SendGridEmailSender> _logger;
        private readonly SendGridConfiguration _configuration;
        private readonly ISendGridClient _client;

        public SendGridEmailSender(ILogger<SendGridEmailSender> logger, ISendGridClient client, SendGridConfiguration configuration)
        {
            _logger = logger;
            _client = client;
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string toName, string subject, string htmlMessage)
        {
            var apiKey = _configuration.ApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(_configuration.SourceEmail, _configuration.SourceName);
            var to = new EmailAddress(toEmail, toName);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
