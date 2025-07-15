using MassTransit;
using MessagingContracts;

namespace ProductsConsumer.Consumers;

public class ProductDeletedConsumer : IConsumer<ProductDeleted>
{
    private readonly ILogger<ProductUpdatedConsumer> _logger;
    public ProductDeletedConsumer(ILogger<ProductUpdatedConsumer> logger)
    {
        _logger = logger;
    }
    public Task Consume(ConsumeContext<ProductDeleted> context)
    {
        _logger.LogInformation(context.Message.ToString());
        return Task.CompletedTask;
    }
}
