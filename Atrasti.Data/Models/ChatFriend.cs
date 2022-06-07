using System.Data;

namespace Atrasti.Data.Models
{
    public class ChatFriend : ISerializable
    {
        public int ChatId { get; private set; }
        
        public int FriendId { get; private set; }
        
        public string FriendCompany { get; private set; }
        
        public string FriendLogo { get; private set; }
        
        public void Serialize(IDataReader reader)
        {
            ChatId = reader.GetData<int>("ChatId");
            FriendId = reader.GetData<int>("FriendId");
            FriendCompany = reader.GetData<string>("FriendCompany");
            FriendLogo = reader.GetData<string>("FriendLogo");
        }
    }
}