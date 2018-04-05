using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Services.HttpService;

namespace QTB3.Client.Common.Services.HttpService
{
    public class HttpServiceAddAuthorizationHeader : IHttpService
    {
        public const string BearerHeader = "Bearer";
        private readonly IJwtTokenSource _tokenSource;
        private readonly IHttpService _next;

        public HttpServiceAddAuthorizationHeader
        (
            IJwtTokenSource tokenSource,
            IHttpService next
        )
        {
            _tokenSource = tokenSource ?? throw new ArgumentNullException(nameof(tokenSource));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            var token = await _tokenSource.GetTokenAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue(BearerHeader, token);
            return await _next.SendAsync(request);
        }
    }
}
