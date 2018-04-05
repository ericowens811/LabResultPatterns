using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Services.Configuration;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Client.Abstractions.Services.LinkService;
using QTB3.Client.Common.Services.Exceptions;

namespace QTB3.Client.Common.Services.LinkService
{
    public class LinkService : ILinkService
    {
        public const string LinkTemplate = "Link-Template";
        private readonly IEndPoint _apiEndpoint;
        private readonly IHttpRequestBuilder _httpRequestBuilder;
        private readonly IHttpClient _httpClient;

        public LinkService
        (
            IEndPoint apiEndpoint,
            IHttpRequestBuilder httpRequestBuilder,
            IHttpReadClient httpClient 
        )
        {
            _apiEndpoint = apiEndpoint ?? throw new ArgumentNullException(nameof(apiEndpoint));
            _httpRequestBuilder = httpRequestBuilder ?? throw new ArgumentNullException(nameof(httpRequestBuilder));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<string> GetLinksAsync()
        {
            var message = await _httpRequestBuilder.BuildAsync(HttpMethod.Head, _apiEndpoint.Url);
            using (var response = await _httpClient.SendAsync(message))
            {
                if (response.IsSuccessStatusCode)
                {
                    response.Headers.TryGetValues(LinkTemplate, out var links);
                    return links.FirstOrDefault();
                }
                throw new LinksException();
            }
        }
    }
}
