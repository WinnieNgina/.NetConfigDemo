using Carter;
using DemoPractise.Controllers;
using DemoPractise.Data;
using DemoPractise.Interfaces;
using DemoPractise.OpenTelementry;
using DemoPractise.Services;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using System.Threading.Channels;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
        .AddSource(DiagnosticConfig.Source.Name)
        .AddSource(MassTransit.Logging.DiagnosticHeaders.DefaultListenerName);
    })
    .WithMetrics(metrics =>
    {
        metrics.AddMeter("Microsoft.AspNetCore.Hosting");
        metrics.AddMeter("Microsoft.AspNetCore.Server.Kestrel");
        metrics.AddMeter("System.Net.Http");
        metrics.AddPrometheusExporter();
        metrics.AddOtlpExporter();
    });
builder.Logging.AddOpenTelemetry(options => {
    options.AddOtlpExporter();
});
builder.Services.AddControllers();
builder.Services.AddCarter();

// Add services to the container.
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IWebHookSubSubscriptionRepository, WebHookSubSubscriptionRepository>();
builder.Services.AddHttpClient(); // Registers IHttpClientFactory
builder.Services.AddScoped<WebhookDispatcher>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//builder.Services.AddHostedService<WebhookProcessor>();
//builder.Services.AddSingleton(_ =>
//{
//    return Channel.CreateBounded<WebhookDispatched>(new BoundedChannelOptions(100)
//    {
//        FullMode = BoundedChannelFullMode.Wait
//    });
//});
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    //x.UsingAmazonSqs((context, cfg) =>
    //{
    //    cfg.Host("us-east-1", _ => { });
    //    cfg.ConfigureEndpoints(context);
    //});
    
    x.AddConsumer<WebHookDispatchedConsumer>();
    x.AddConsumer<WebhookTriggeredConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
// app.MapProductsEndpoints(); - using static method
app.MapCarter(); // Scans assembly to look for implementation of ICarter module interface
app.MapPrometheusScrapingEndpoint();

app.Run();
