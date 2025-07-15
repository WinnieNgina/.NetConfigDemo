using DemoPractise.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http;
using System.Text.Json;
using DemoPractise.Data;
using DemoPractise.Interfaces;

namespace DemoPractise.Services;

internal sealed class WebhookTriggeredConsumer : IConsumer<WebhookTriggered>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly DataContext _dbcontext;

    public WebhookTriggeredConsumer(IHttpClientFactory httpClientFactory, DataContext dbcontext)
    {
        _httpClientFactory = httpClientFactory;
        _dbcontext = dbcontext;
    }

    public async Task Consume(ConsumeContext<WebhookTriggered> context)
    {
        using var client = _httpClientFactory.CreateClient();
        var payload = new WebHookPayload
        {
            Id = Guid.NewGuid().ToString(),
            EventType = context.Message.EventType,
            SubscriptionId = context.Message.SubscriptionId,
            TimeStamp = DateTime.UtcNow,
            Data = context.Message.Data
        };
        var jsonPayload = JsonSerializer.Serialize(payload);
        try
        {
            var response = await client.PostAsJsonAsync(context.Message.WebhookUrl, payload);
            response.EnsureSuccessStatusCode();
            var attempt = new WebHookDeliveryAttempt
            {
                Id = Guid.NewGuid().ToString(),
                SubscriptionId = context.Message.SubscriptionId,
                Payload = jsonPayload,
                ResponseStatusCode = (int)response.StatusCode,
                Success = response.IsSuccessStatusCode,
                TimeStamp = DateTime.UtcNow
            };
            _dbcontext.WebhooksDeliveryAttempts.Add(attempt);
            await _dbcontext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            var attempt = new WebHookDeliveryAttempt
            {
                Id = Guid.NewGuid().ToString(),
                SubscriptionId = context.Message.SubscriptionId,
                Payload = jsonPayload,
                ResponseStatusCode = null,
                Success = false,
                TimeStamp = DateTime.UtcNow
            };
            _dbcontext.WebhooksDeliveryAttempts.Add(attempt);
            await _dbcontext.SaveChangesAsync();
        }
    }

}
