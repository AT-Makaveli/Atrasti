using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Atrasti.Spa.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            /*IClientProxy clientProxy = Clients.Client("");
            clientProxy.SendCoreAsync("ReceiveMessage", new[] {"", ""});*/
            
            Console.WriteLine(user);
            Console.WriteLine(message);
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}