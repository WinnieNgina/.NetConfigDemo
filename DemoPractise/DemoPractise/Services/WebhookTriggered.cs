namespace DemoPractise.Services;

public record WebhookTriggered(string SubscriptionId, string EventType, string WebhookUrl, object Data);
