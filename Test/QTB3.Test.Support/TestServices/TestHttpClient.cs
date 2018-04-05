using QTB3.Client.Abstractions.Services;
using System.Net.Http;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Services.HttpService;

namespace QTB3.Test.Support.TestServices
{
    public class TestHttpClient: IHttpReadClient, IHttpWriteClient
    {
        private readonly HttpClient _client;

        public TestHttpClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return await _client.SendAsync(request);
        }
    }
}