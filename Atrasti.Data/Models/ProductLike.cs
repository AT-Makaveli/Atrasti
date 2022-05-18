using System.Data;

namespace Atrasti.Data.Models
{
    public class ProductLike : ISerializable
    {
        public uint RefId { get; set; }

        public int UserId { get; set; }

        public void Serialize(IDataReader reader)
        {
            RefId = reader.GetData<uint>("RefId");
            UserId = reader.GetData<int>("UserId");
        }
    }
}
