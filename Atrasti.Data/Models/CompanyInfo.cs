using System.Data;

namespace Atrasti.Data.Models
{
    public class CompanyInfo : ISerializable
    {
        public int RefId { get; set; }

        public string BusinessType { get; set; }

        public string MainProducts { get; set; }

        public string MainMarkets { get; set; }

        public int YearEstablished { get; set; }
        
        public string Certificates { get; set; }

        public string Capacity { get; set; }

        public void Serialize(IDataReader reader)
        {
            RefId = reader.GetData<int>("RefId");
            BusinessType = reader.GetData<string>("BusinessType");
            MainProducts = reader.GetData<string>("MainProducts");
            MainMarkets = reader.GetData<string>("MainMarkets");
            YearEstablished = reader.GetData<int>("YearEstablished");
            Certificates = reader.GetData<string>("Certificates");
            Capacity = reader.GetData<string>("Capacity");
        }
    }
}
