using System;
using System.Net.Http;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Services.HttpService;

namespace QTB3.Client.Common.Services.HttpService
{
    public class HttpServiceSendRequest : IHttpService
    {
        private readonly IHttpClient _httpClient;

        public HttpServiceSendRequest(IHttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            return await _httpClient.SendAsync(request);
        }
    }
}
