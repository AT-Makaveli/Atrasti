using System.Collections.Generic;

namespace Atrasti.API.Models.Chat
{
    public class ChatMessages_Res
    {
        public ICollection<ChatMessage_Res> Messages { get; set; }
    }
}