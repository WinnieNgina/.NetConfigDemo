using Carter;
using DemoPractise.Interfaces;
using DemoPractise.Models;
using DemoPractise.Records.Product;

namespace DemoPractise.Controllers;

public class WebHookEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/webhooks/");
        group.MapPost("", CreateWebHook)
          .WithName(nameof(CreateWebHook));
    }
    public static async Task<IResult> CreateWebHook(IWebHookSubSubscriptionRepository webHookSubSubscriptionRepository,CreateWebHookRequest request)
    {
        WebHookSubscription subscription = new WebHookSubscription
        {
            EventType = request.EventType,
            WebHookUrl = request.WebhookUrl,
        };
        await webHookSubSubscriptionRepository.AddWebHookAsync(subscription);
        return Results.Ok(subscription);
    }
}
