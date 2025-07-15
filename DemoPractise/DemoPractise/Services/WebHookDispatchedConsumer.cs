using DemoPractise.Data;
using DemoPractise.Interfaces;
using MassTransit;

namespace DemoPractise.Services;

internal sealed class WebHookDispatchedConsumer : IConsumer<WebhookDispatched>
{
    private readonly DataContext _dbcontext;
    private readonly IWebHookSubSubscriptionRepository _subSubscriptionRepository;
   
    public WebHookDispatchedConsumer(IWebHookSubSubscriptionRepository subSubscriptionRepository, DataContext dbcontext)
    {
        _subSubscriptionRepository = subSubscriptionRepository;
        _dbcontext = dbcontext;
    }

    public async Task Consume(ConsumeContext<WebhookDispatched> context)
    {
        var message = context.Message;
        var subscriptions = await _subSubscriptionRepository.GetByEventType(message.EventType);
        foreach (var  s in subscriptions)
        {
            await  context.Publish(new WebhookTriggered(
            
                s.SubscriptionId,
                s.EventType,
                s.WebHookUrl,
                message.Data));
        }
    }
}
