namespace DemoPractise.Services;

public sealed record WebhookDispatched(string EventType, object Data);
