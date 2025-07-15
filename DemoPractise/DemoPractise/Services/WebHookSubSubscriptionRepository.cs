using DemoPractise.Data;
using DemoPractise.Interfaces;
using DemoPractise.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoPractise.Services;

public class WebHookSubSubscriptionRepository : IWebHookSubSubscriptionRepository
{
    private readonly DataContext _context;
    public WebHookSubSubscriptionRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<bool> AddWebHookAsync(WebHookSubscription order)
    {
        await _context.Webhooks.AddAsync(order);
        return await _context.SaveChangesAsync() > 1;
    }
    public async Task<IEnumerable<WebHookSubscription>> GetByEventType(string eventType)
    {
        return await _context.Webhooks.Where(w =>  w.EventType == eventType).AsNoTracking()
                .ToListAsync();
    }
}
