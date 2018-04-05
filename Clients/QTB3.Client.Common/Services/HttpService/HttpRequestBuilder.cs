using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Client.Abstractions.Services.Serialization;

namespace QTB3.Client.Common.Services.HttpService
{
    public class HttpRequestBuilder : IHttpRequestBuilder
    {
        public const string LrpMediaTypeV2 = "application/vnd.lrp.v1+json";
        public const string BearerHeader = "Bearer";

        private readonly IJwtTokenSource _tokenSource;
        private readonly IContentSerializer _serializer;

        public HttpRequestBuilder
        (
            IJwtTokenSource tokenSource,
            IContentSerializer serializer
        )
        {
            _tokenSource = tokenSource;
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        private async Task<HttpRequestMessage> BuildBaseRequest(HttpMethod method, string url, string supportedMedia = LrpMediaTypeV2)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)) throw new ArgumentException(nameof(url));
            var token = await _tokenSource.GetTokenAsync();
            var message = new HttpRequestMessage(method, url);
            message.Headers.Authorization = new AuthenticationHeaderValue(BearerHeader, token);

            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(supportedMedia));

            return message;
        }

        public async Task<HttpRequestMessage> BuildAsync(HttpMethod method, string url)
        {
            return await BuildBaseRequest(method, url);
        }

        public async Task<HttpRequestMessage> BuildAsync<T>(HttpMethod method, string url, T item) where T : class
        {
            if(item == null) throw new ArgumentNullException(nameof(item));
            var message = await BuildBaseRequest(method, url);
            _serializer.Serialize(message, item);
            return message;
        }
    }
}
