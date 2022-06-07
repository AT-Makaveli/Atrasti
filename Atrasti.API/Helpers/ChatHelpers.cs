using System.Collections.Generic;
using Atrasti.API.Models.Chat;
using Atrasti.Data.Models;

namespace Atrasti.API.Helpers
{
    public static class ChatHelpers
    {
        public static IList<ChatFriend_Res> MapChatFriends(this IList<ChatFriend> friends)
        {
            IList<ChatFriend_Res> res = new List<ChatFriend_Res>();
            foreach (ChatFriend friend in friends)
            {
                res.Add(friend.MapChatFriend());
            }

            return res;
        }

        public static ChatFriend_Res MapChatFriend(this ChatFriend friend)
        {
            return new ChatFriend_Res()
            {
                ChatId = friend.ChatId,
                FriendId = friend.FriendId,
                FriendCompany = friend.FriendCompany,
                FriendLogo = friend.FriendLogo
            };
        }

        public static IList<ChatMessage_Res> MapChatMessages(this IEnumerable<ChatMessage> messages)
        {
            var ret = new List<ChatMessage_Res>();
            foreach (ChatMessage message in messages)
            {
                ret.Add(message.MapChatMessage());
            }

            return ret;
        }
        
        public static ChatMessage_Res MapChatMessage(this ChatMessage message)
        {
            return new ChatMessage_Res()
            {
                Id = message.Id,
                ChatMessage = message.Message,
                Created = message.Created,
                SenderId = message.SenderId,
                Author = message.Author
            };
        }
    }
}