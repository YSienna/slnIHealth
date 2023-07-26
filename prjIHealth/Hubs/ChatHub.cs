using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using prjiHealth.Models;
using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace CoreMVC_SignalR_Chat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string CoachContactId, bool? IsCoach, string ContactText)
        {
            await Clients.All.SendAsync("ReceiveMessage", CoachContactId, IsCoach, ContactText);
        }
        
    }

}