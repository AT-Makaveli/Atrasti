using System;
using System.Data;

namespace Atrasti.Data.Models
{
    public class ForumPost : ISerializable
    {
        public uint Id { get; set; }

        public int AuthorId { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public string CompanyName { get; set; }

        public uint ThreadId { get; set; }

        public void Serialize(IDataReader reader)
        {
            Id = reader.GetData<uint>("Id");
            AuthorId = reader.GetData<int>("AuthorId");
            Text = reader.GetData<string>("Text");
            Date = reader.GetDate("Date");
            ThreadId = reader.GetData<uint>("ThreadId");

            CompanyName = reader.GetData<string>("Company");
        }
    }
}
