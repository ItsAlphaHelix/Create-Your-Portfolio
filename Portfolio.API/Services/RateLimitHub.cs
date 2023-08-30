namespace Portfolio.API.Services
{
    using Microsoft.AspNetCore.SignalR;
    using System.Collections.Concurrent;
    public class RateLimitHub : Hub
    {
        private static readonly ConcurrentDictionary<string, DateTime> LastInvokeTimes = new ConcurrentDictionary<string, DateTime>();

        public async Task<bool> CanInvokeMethod(string userId)
        {
            if (LastInvokeTimes.TryGetValue(userId, out var lastInvokeTime)
                && DateTime.UtcNow - lastInvokeTime < TimeSpan.FromSeconds(30))
            {
                return false;
            }

            LastInvokeTimes.AddOrUpdate(userId, DateTime.UtcNow, (_, _) => DateTime.UtcNow);

            return true;
        }
    }
}
