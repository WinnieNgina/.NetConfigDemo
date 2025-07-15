namespace DemoPractise.Services;

public class WebHookPayload
{
    public string Id { get; set; }
    public string EventType { get; set; }
    public string SubscriptionId { get; set; }
    public DateTime TimeStamp { get; set; }
    public object Data { get; set; }
}