using System.Net.Http;
using System.Threading.Tasks;

namespace QTB3.Client.Abstractions.Services.Serialization
{
    public interface IContentDeserializer
    {
        Task<T> DeserializeAsync<T>(HttpContent content);
    }
}
