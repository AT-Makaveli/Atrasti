using System;

namespace Atrasti.API.Models.Chat
{
    public class ChatMessage_Res
    {
        public long Id { get; set; }
        public int SenderId { get; set; }
        public string ChatMessage { get; set; }
        public DateTime Created { get; set; }
        public bool FromMe { get; set; }
        public string Author { get; set; }
        public bool HasBeenRead { get; set; }
    }
}