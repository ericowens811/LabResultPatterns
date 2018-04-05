using System;
using System.Net.Http;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Services.HttpService;

namespace QTB3.Client.Common.Services.HttpService
{
    public class HttpReadService : IHttpReadService
    {
        private readonly IHttpService _next;

        public HttpReadService(IHttpService next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            if(request == null) throw new ArgumentNullException(nameof(request));
            return await _next.SendAsync(request);
        }
    }
}
