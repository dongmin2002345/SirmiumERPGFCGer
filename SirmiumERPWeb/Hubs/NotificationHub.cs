using Microsoft.AspNetCore.SignalR;
using ServiceInterfaces.ViewModels.Common.CallCentars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SirmiumERPWeb.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task OnNotification(CallCentarViewModel callCenter)
        {
            string kome = "Korisnik_" + (callCenter?.User?.Id ?? 0);
            if (!String.IsNullOrEmpty(kome))
            {
                var group = Clients.Group(kome);
                if (group != null)
                {
                    await Clients.Group(kome).SendAsync("OnNotification", callCenter);
                }
            }
            else
                await Clients.All.SendAsync("OnNotification", callCenter);

            //await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task JoinNotificationGroup(int userId)
        {
            string userJoined = $"Korisnik_{userId}";
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, userJoined);
            await Clients.Group(userJoined).SendAsync("AcceptedToNotificationChannel", userId);
        }
    }
}
