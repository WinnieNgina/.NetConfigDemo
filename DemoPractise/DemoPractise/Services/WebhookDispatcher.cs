﻿using DemoPractise.Data;
using DemoPractise.Interfaces;
using DemoPractise.Models;
using DemoPractise.OpenTelementry;
using MassTransit;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Channels;

namespace DemoPractise.Services;

public sealed class WebhookDispatcher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public WebhookDispatcher(IPublishEndpoint publishEndpoint)
    {
        
        _publishEndpoint = publishEndpoint;
    }
    public async Task DispatchAsync<T>(string eventType, T data)
        where T : notnull
    {
        using Activity? activity = DiagnosticConfig.Source.StartActivity($"{eventType} dispatch webhook");
        activity?.AddTag("event.type", eventType);
        await _publishEndpoint.Publish(new WebhookDispatched(eventType, data));
    }
}
