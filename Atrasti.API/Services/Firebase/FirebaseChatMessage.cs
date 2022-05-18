using System.Collections.Generic;
using System.Globalization;
using Atrasti.API.Models.Chat;
using FirebaseAdmin.Messaging;

namespace Atrasti.API.Services.Firebase
{
    public class FirebaseChatMessage : IFirebaseMessageSerializer
    {
        private ChatMessage_Res ChatMessageRes { get; }
        private string Token { get; }

        public FirebaseChatMessage(ChatMessage_Res chatMessage, string token)
        {
            ChatMessageRes = chatMessage;
            Token = token;
        }

        public Message Serialize()
        {
            var data = new Dictionary<string, string>
            {
                {"chat-message", ChatMessageRes.ChatMessage},
                {"chat-from-user-id", ChatMessageRes.SenderId.ToString()},
                {"chat-from-me", false.ToString()},
                {"chat-created", ChatMessageRes.Created.ToString(CultureInfo.CurrentCulture)},
                {"chat-author", ChatMessageRes.Author},
                {"header", "CHAT_MESSAGE"}
            };

            var notification = new Notification();
            notification.Title = "New message!";
            notification.Body = ChatMessageRes.Author + " sent you a message.";

            return new Message()
            {
                Token = Token,
                Data = data,
                Notification = notification,
            };
        }
    }
}