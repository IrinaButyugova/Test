using Microsoft.AspNetCore.SignalR;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SignalRApp
{
    public class ChatHub : Hub
    {
        public async Task Send(string message)
        {
            await this.Clients.All.SendAsync("Send", message, Context.ConnectionId);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} вошел в чат");

            var context = Context.GetHttpContext();
            if (context.Request.Cookies.ContainsKey("name"))
            {
                string userName;
                if (context.Request.Cookies.TryGetValue("name", out userName))
                {
                    Debug.WriteLine($"name = {userName}");
                }
            }
            Debug.WriteLine($"UserAgent = {context.Request.Headers["User-Agent"]}");
            Debug.WriteLine($"RemoteIpAddress = {context.Connection.RemoteIpAddress}");

            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} покинул в чат");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
