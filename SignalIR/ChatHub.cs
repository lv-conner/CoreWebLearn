using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace SignalIR
{
    public class ChatHub:Hub
    {
        public static readonly ConcurrentBag<ChatUser> chatUsers = new ConcurrentBag<ChatUser>();
        public override Task OnConnectedAsync()
        {
            chatUsers.Add(new ChatUser()
            {
                ConnectionId = Context.ConnectionId
            });
            Clients.Caller.SendAsync("ReceiveMessage", this.Context.ConnectionId,"welcome");
            return base.OnConnectedAsync();
        }

        public async Task SetName(string userName)
        {
            if(chatUsers.FirstOrDefault(p=>p.ChatUserName == userName)!= null)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", userName + "has been used by another user");
                return;
            }
            chatUsers.First(p => p.ConnectionId == Context.ConnectionId).ChatUserName = userName;
            await Clients.Caller.SendAsync("ReceiveMessage", "set name success");
        }

        public async Task SendToUser(string userName,string message)
        {
            var user = chatUsers.FirstOrDefault(p => p.ChatUserName == userName);
            var caller = chatUsers.First(p => p.ConnectionId == Context.ConnectionId);
            if(user != null)
            {
                await Clients.Client(user.ConnectionId).SendAsync("ReceiveMessage", caller, message);
            }
            else
            {
                await Clients.Client(user.ConnectionId).SendAsync("ReceiveMessage", caller, "can not find this user");
            }
        }

        public async Task CreateGroup(string groupName)
        {
            
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageToCaller(string message)
        {
            return Clients.Caller.SendAsync("ReceiveMessage", message);
        }

        public Task SendMessageToGroups(string message)
        {
            List<string> groups = new List<string>() { "SignalR Users" };
            return Clients.Groups(groups).SendAsync("ReceiveMessage", message);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }
    }


    public class ChatUser
    {
        public string ConnectionId { get; set; }
        public string ChatUserName { get; set; }
        public string ConnnectTime { get; set; }
    }


    public interface IChatClient
    {
        Task ReceiveMessage(string user, string message);
    }
}
