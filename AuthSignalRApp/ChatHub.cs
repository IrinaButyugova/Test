﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AuthSignalRApp
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task Send(string message, string to)
        {
            var userName = Context.User.Identity.Name;

            if (Context.UserIdentifier != to)
            {
                await Clients.User(Context.UserIdentifier).SendAsync("Receive", message, userName);
            }  
            await Clients.User(to).SendAsync("Receive", message, userName);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Notify", $"Приветствуем {Context.UserIdentifier}");
            await base.OnConnectedAsync();
        }
    }
}
