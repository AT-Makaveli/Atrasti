using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Atrasti.Data.Models;

namespace Atrasti.Data.Core
{
    public interface IChatRepository
    {
        Task<IList<ChatMessage>> FetchMessagesAsync(int chatId);
        
        Task<int> WriteMessageAsync(ChatMessage chatMessage);
        
        Task<IList<ChatFriend>> FetchFriendsAsync(int userId);

        Task<IList<ChatFriend>> FetchFriendRequestsAsync(int userId);
        
        Task<ChatFriend> FetchFriendAsync(int userId, int friendId);

        Task<int> AddFriend(int userId, int friendId);
    }
}