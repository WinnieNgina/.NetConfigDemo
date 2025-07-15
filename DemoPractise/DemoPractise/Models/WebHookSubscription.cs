using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DemoPractise.Models;

public class WebHookSubscription
{
    [Key]
    public string SubscriptionId { get; set; } = Guid.NewGuid().ToString();
    public string EventType { get; set; }
    public string WebHookUrl { get; set; }
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
}
public sealed record CreateWebHookRequest(string EventType, string WebhookUrl);
