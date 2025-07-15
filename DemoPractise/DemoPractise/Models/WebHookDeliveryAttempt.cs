using System.ComponentModel.DataAnnotations;

namespace DemoPractise.Models;

public class WebHookDeliveryAttempt
{
    [Key]
    public string Id { get; set; }
    public string SubscriptionId { get; set; }
    public string Payload { get; set; }
    public int? ResponseStatusCode { get; set; }
    public bool Success { get; set; }
    public DateTime TimeStamp { get; set; }
}
