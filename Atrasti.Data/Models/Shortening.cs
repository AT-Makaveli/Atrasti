using System.Data;

namespace Atrasti.Data.Models
{
    public class Shortening : ISerializable
    {
        public string Shortened { get; set; }
        
        public string Original { get; set; }
        
        public void Serialize(IDataReader reader)
        {
            Shortened = reader.GetData<string>("shortening");
            Original = reader.GetData<string>("original");
        }
    }
}