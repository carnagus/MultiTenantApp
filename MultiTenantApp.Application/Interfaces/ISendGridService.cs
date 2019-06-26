using System.IO;
using System.Threading.Tasks;
using StrongGrid.Models.Webhooks;

namespace MultiTenantApp.Application.Interfaces
{
    public interface ISendGridService
    {
        Task SendToSingleRecipientAsync(string to, string from, string subject, string htmlcontent, string textContent);
        Task<Event[]> ParseWebhookEventsAsync(Stream requestBody);
        Task<InboundEmail> ParseInboundEmailWebhook(Stream requestBody);
    }
}