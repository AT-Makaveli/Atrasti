using System.Collections.Generic;

namespace Atrasti.API.Models.Chat
{
    public class ChatFriends_Res
    {
        public ICollection<ChatFriend_Res> Friends { get; set; }
    }
}