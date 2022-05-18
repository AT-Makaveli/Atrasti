using System.Data;

namespace Atrasti.Data
{
    internal interface ISerializable
    {
        void Serialize(IDataReader reader);
    }
}
