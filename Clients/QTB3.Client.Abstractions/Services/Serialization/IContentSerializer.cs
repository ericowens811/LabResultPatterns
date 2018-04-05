
using System.Net.Http;

namespace QTB3.Client.Abstractions.Services.Serialization
{
    public interface IContentSerializer
    {
        void Serialize<T>(HttpRequestMessage message, T item);
    }
}
