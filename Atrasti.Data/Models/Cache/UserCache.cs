using System;
using System.Data;

namespace Atrasti.Data.Models.Cache
{
    public class UserCache : ISerializable
    {
        public int RefId { get; set; }
        public string Value { get; set; }
        public CacheType Type { get; set; }
        
        public void Serialize(IDataReader reader)
        {
            RefId = reader.GetData<int>("RefId");
            Value = reader.GetData<string>("Value");
            Type = Enum.Parse<CacheType>(reader.GetData<sbyte>("Type").ToString());
        }
    }
}