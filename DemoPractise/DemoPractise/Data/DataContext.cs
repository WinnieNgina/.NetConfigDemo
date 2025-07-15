using DemoPractise.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoPractise.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
    public DbSet<Product> Products { get; set; }
    public DbSet<WebHookSubscription> Webhooks { get; set; }
    public DbSet<WebHookDeliveryAttempt> WebhooksDeliveryAttempts { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WebHookDeliveryAttempt>(builder =>
        {
            builder.HasOne<WebHookSubscription>()
                .WithMany()
                .HasForeignKey(d => d.SubscriptionId);
        });
    }

}
