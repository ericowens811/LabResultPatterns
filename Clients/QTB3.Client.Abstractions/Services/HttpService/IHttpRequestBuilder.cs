using System.Net.Http;
using System.Threading.Tasks;

namespace QTB3.Client.Abstractions.Services.HttpService
{
    public interface IHttpRequestBuilder
    {
        Task<HttpRequestMessage> BuildAsync(HttpMethod method, string url);
        Task<HttpRequestMessage> BuildAsync<T>(HttpMethod method, string url, T item) where T: class;
    }
}
