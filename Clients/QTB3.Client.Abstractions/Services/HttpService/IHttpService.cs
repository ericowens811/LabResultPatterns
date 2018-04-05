using System.Net.Http;
using System.Threading.Tasks;

namespace QTB3.Client.Abstractions.Services.HttpService
{
    public interface IHttpService
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}
