namespace Portfolio.API.Services
{
    using Microsoft.AspNetCore.SignalR;
    using System.Collections.Concurrent;
    public class RateLimitHub : Hub
    {
        private static readonly ConcurrentDictionary<string, DateTime> LastInvokeTimes = new ConcurrentDictionary<string, DateTime>();

        /// <summary>
        /// This method establishes a rate limit based on the user's userID for API calls. The API will be invoked solely once and subsequently reactivated after a 30-day interval.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
