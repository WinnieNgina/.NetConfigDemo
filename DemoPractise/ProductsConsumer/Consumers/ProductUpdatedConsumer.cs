using MassTransit;
using MessagingContracts;

namespace ProductsConsumer.Consumers;

public class ProductUpdatedConsumer : IConsumer<ProductUpdated>
{
    private readonly ILogger<ProductUpdatedConsumer> _logger;
    public ProductUpdatedConsumer(ILogger<ProductUpdatedConsumer> logger)
    {
        _logger = logger;
    }
    public Task Consume(ConsumeContext<ProductUpdated> context)
    {
        _logger.LogInformation(context.Message.ToString());
        return Task.CompletedTask;
    }
}
