using System;
using System.Collections.Generic;
using System.Data;

namespace Atrasti.Data.Models
{
    public class Company : ISerializable
    {
        public int RefId { get; set; }

        public string Address { get; set; }

        public string? City { get; set; }
        
        public string State { get; set; }

        public string Country { get; set; }

        public string Website { get; set; }

        public string CompanyDesc { get; set; }

        public CompanyInfo CompanyInfo { get; set; }

        public ICollection<Product> Products { get; set; }

        public void Serialize(IDataReader reader)
        {
            RefId = reader.GetData<int>("RefId");
            Address = reader.GetData<string>("Address");
            City = reader.GetDataNullable("City");
            State = reader.GetData<string>("State");
            Country = reader.GetData<string>("Country");
            Website = reader.GetData<string>("Website");
            CompanyDesc = reader.GetData<string>("CompanyDesc");

            if (reader["BusinessType"] != DBNull.Value)
            {
                CompanyInfo = new CompanyInfo();
                CompanyInfo.Serialize(reader);
            }
        }
    }
}
