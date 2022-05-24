using System.Data;

namespace Atrasti.Data.Models
{
    public class BaseCategory : ISerializable
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public void Serialize(IDataReader reader)
        {
            Id = reader.GetData<int>("Id");
            Title = reader.GetData<string>("Title");
        }
    }
}