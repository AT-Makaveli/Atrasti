using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Atrasti.Data.Models
{
    public class Product : ISerializable
    {
        public uint Id { get; set; }

        public int CompanyId { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public IList<string> Tags { get; set; }

        public IList<string> PhoneticTags { get; set; }

        public IList<int> ProductLikes { get; set; }

        public bool IsHeartPressed { get; set; } = false;

        public int ProductCategory { get; set; }
        
        public void Serialize(IDataReader reader)
        {
            ProductLikes = new List<int>();
            Id = reader.GetData<uint>("Id");
            CompanyId = reader.GetData<int>("CompanyId");
            Title = reader.GetData<string>("Title");
            Description = reader.GetData<string>("Description");
            Tags = JsonConvert.DeserializeObject<List<string>>(reader.GetData<string>("Tags"));
            PhoneticTags = reader.GetData<string>("PhoneticTags").Split(' ').ToList();
            Image = Id + ".png";
        }

        public void SerializeLikes(IDataReader reader)
        {
            if (reader["UserId"] is int)
            {
                ProductLikes.Add(reader.GetData<int>("UserId"));
            }
        }
    }
}