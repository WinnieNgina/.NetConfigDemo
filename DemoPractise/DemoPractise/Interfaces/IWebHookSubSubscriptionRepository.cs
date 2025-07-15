using DemoPractise.Models;

namespace DemoPractise.Interfaces;

public interface IWebHookSubSubscriptionRepository
{
    Task<bool> AddWebHookAsync(WebHookSubscription order);
    Task<IEnumerable<WebHookSubscription>> GetByEventType(string eventType);
}
