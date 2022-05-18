using System.Data;

namespace Atrasti.Data.Models
{
    public class Agent : ISerializable
    {
        public int RefId { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Country { get; set; }
        public string? County { get; set; }
        public string? City { get; set; }
        public string? Website { get; set; }
        public string? Description { get; set; }
        public string? BusinessSector { get; set; }
        public string? MainProducts { get; set; }
        public string? MainMarkets { get; set; }
        public string? YearsOfExperience { get; set; }
        public string? Certificates { get; set; }

        public void Serialize(IDataReader reader)
        {
            RefId = reader.GetData<int>("ref_id");
            Address = reader.GetDataNullable("address");
            PhoneNumber = reader.GetDataNullable("phone_number");
            Country = reader.GetDataNullable("country");
            County = reader.GetDataNullable("county");
            City = reader.GetDataNullable("city");
            Website = reader.GetDataNullable("website");
            Description = reader.GetDataNullable("description");
            BusinessSector = reader.GetDataNullable("b_sector");
            MainProducts = reader.GetDataNullable("main_products");
            MainMarkets = reader.GetDataNullable("main_markets");
            YearsOfExperience = reader.GetDataNullable("years_experience");
            Certificates = reader.GetDataNullable("certificates");
        }
    }
}