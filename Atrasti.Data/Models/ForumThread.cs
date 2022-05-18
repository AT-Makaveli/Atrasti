using System;
using System.Data;

namespace Atrasti.Data.Models
{
    public class ForumThread : ISerializable
    {
        public uint Id { get; set; }

        public int AuthorId { get; set; }

        public string Title { get; set; }

        public DateTime PublishDate { get; set; }

        public int Views { get; set; }

        public AtrastiUser Author { get; set; }

        public void Serialize(IDataReader reader)
        {
            Id = reader.GetData<uint>("Id");
            AuthorId = reader.GetData<int>("Author");
            Title = reader.GetData<string>("Title");
            PublishDate = reader.GetDate("Date");
            Views = reader.GetData<int>("Views");
        }
    }
}
