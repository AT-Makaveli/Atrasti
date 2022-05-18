using System;
using System.IO;
using System.Threading.Tasks;
using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Atrasti.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Atrasti.WebSocket.Hubs
{
    //TODO: Figure out how the fuck this works... [Authorize(AuthenticationSchemes = "Basic")]
    public class ChatHub : Hub
    {
        public static readonly ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();

        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AtrastiUser> _userManager;

        public ChatHub(IChatRepository chatRepository, UserManager<AtrastiUser> userManager,
            IUserRepository userRepository)
        {
            _chatRepository = chatRepository;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<object> SendChatMessage(string to, string message)
        {
            if (!int.TryParse(to, out int toId)) return new {errorCode = -1};

            AtrastiUser fromUser = await _userManager.GetUserAsync(Context.User);
            AtrastiUser toUser = await _userRepository.FindUserById(toId);

            if (fromUser == null || toUser == null) return new {errorCode = -1};

            ChatFriend friend = await _chatRepository.FetchFriendAsync(fromUser.Id, toId);

            if (friend == null) return new {errorCode = -1};

            int timeStamp = NumberUtils.UnixTimeStamp();

            ulong chatId = await _chatRepository.WriteMessageAsync(fromUser.Id, friend.ChatId, message, timeStamp,
                (int) ChatType.CHAT);

            foreach (var connectionId in _connections.GetConnections(toUser.Email))
            {
                await Clients.Client(connectionId)
                    .SendAsync("ReceiveMessage", chatId, fromUser.Id, fromUser.Company, message, timeStamp);
            }

            return new ChatObject(chatId, fromUser.Id, fromUser.Company, message, timeStamp);
        }

        public struct ChatObject
        {
            public int ErrorCode { get; }

            public ulong ChatId { get; }

            public int FromId { get; }

            public string FromCompany { get; }

            public string Message { get; }

            public int TimeStamp { get; }

            public ChatObject(ulong chatId, int fromId, string fromCompany, string message, int timeStamp) : this()
            {
                ErrorCode = 0;
                ChatId = chatId;
                FromId = fromId;
                FromCompany = fromCompany;
                Message = message;
                TimeStamp = timeStamp;
            }
        }

        public override Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;
            _connections.Add(name, Context.ConnectionId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception ex)
        {
            string name = Context.User.Identity.Name;

            _connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnectedAsync(ex);
        }
    }
}