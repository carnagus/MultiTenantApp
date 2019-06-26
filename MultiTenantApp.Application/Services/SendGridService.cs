namespace MultiTenantApp.Application.Services
{
    using Microsoft.Extensions.Configuration;
    using MultiTenantApp.Application.Interfaces;
    using StrongGrid;
    using StrongGrid.Models;
    using StrongGrid.Models.Webhooks;
    using System.IO;
    using System.Threading.Tasks;

    //used https://sendgrid.com/docs/for-developers/sending-email/libraries/
    //library https://github.com/Jericho/StrongGrid

    public class SendGridService: ISendGridService
    {
        private readonly string _sengridApiKey;
        private readonly WebhookParser _parser;
        private readonly Client _client;
        public SendGridService(IConfiguration configuration)
        {
            _sengridApiKey = configuration["SengridApiKey"];
            _parser = new WebhookParser();
            _client=new Client(_sengridApiKey);
        }
        public async Task SendToSingleRecipientAsync(string to, string from, string subject, string htmlContent, string textContent)
        {
            var emailAdressTo = new MailAddress(to,to);
            var emailAdressFrom = new MailAddress(from, from);
            await _client.Mail.SendToSingleRecipientAsync(emailAdressTo, emailAdressFrom,subject,htmlContent,textContent);
        }

        public async Task<Event[]> ParseWebhookEventsAsync(Stream requestBody)
        {
            var events = await _parser.ParseWebhookEventsAsync(requestBody).ConfigureAwait(false);

            return events;
        }

        public Task<InboundEmail> ParseInboundEmailWebhook(Stream requestBody)
        {
            var inboundEmail = _parser.ParseInboundEmailWebhook(requestBody);

            return Task.FromResult(inboundEmail);
        }
    }
}