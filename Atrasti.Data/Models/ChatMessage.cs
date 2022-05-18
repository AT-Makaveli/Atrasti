using System;
using System.Data;

namespace Atrasti.Data.Models
{
    public class ChatMessage : ISerializable
    {
        public int Id { get;  set; }
        
        public int SenderId { get; set; }
        
        public int ChatId { get;  set; }
        
        public DateTime Created { get;  set; }
        
        public string Message { get;  set; }
        
        public string Date { get;  set; }
        
        public ChatType ChatType { get;  set; }
        
        public string Author { get; set; }
        
        public bool HasBeenRead { get; set; }
        
        public void Serialize(IDataReader reader)
        {
            Id = reader.GetData<int>("Id");
            SenderId = reader.GetData<int>("SenderId");
            ChatId = reader.GetData<int>("ChatId");
            Created = reader.GetDate("Created");
            Message = reader.GetData<string>("ChatMessage");
            ChatType = (ChatType) reader.GetData<int>("ChatType");
            Author = reader.GetData<string>("Author");
            HasBeenRead = reader.GetData<bool>("HasBeenRead");
        }
    }
}