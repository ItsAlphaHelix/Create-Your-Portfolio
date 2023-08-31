namespace Portfolio.API.Services
{
    using Microsoft.AspNetCore.SignalR.Client;
    public class RateLimitCheckerService
    {
        private readonly HubConnection hubConnection;

        public RateLimitCheckerService()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7126/rateLimitHub")
                .Build();
        }

        /// <summary>
        /// This function start a hub connection and validates the rate limit.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> CheckRateLimitAsync(string userId)
        {
            await this.hubConnection.StartAsync();

            try
            {
                var canInvoke = await this.hubConnection.InvokeAsync<bool>("CanInvokeMethod", userId);
                return canInvoke;
            }
            finally
            {
                await this.hubConnection.DisposeAsync();
            }
        }
    }
}
