namespace Atrasti.API.Models.Chat
{
    public class SendChatMessage_Req
    {
        public int ChatId { get; set; }
        public int FriendId { get; set; }
        public string Message { get; set; }
    }
}