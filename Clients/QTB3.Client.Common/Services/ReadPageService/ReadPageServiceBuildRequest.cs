using System;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Paging;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Client.Abstractions.Services.ReadPageService;
using QTB3.Client.Abstractions.Services.Serialization;
using QTB3.Client.Common.Services.Data;
using QTB3.Client.Common.Services.Exceptions;
using IHttpReadService = QTB3.Client.Abstractions.Services.HttpService.IHttpReadService;

namespace QTB3.Client.Common.Services.ReadPageService
{
    public class ReadPageServiceBuildRequest<TItem> : IReadPageService<TItem>
    {
        private readonly IHttpService _httpService;
        private readonly IContentDeserializer _deserializer;

        public ReadPageServiceBuildRequest
        (
            IHttpReadService httpService,
            IContentDeserializer deserializer
        )
        {
            _httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
            _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
        }

        public async Task<ICollectionPageData<TItem>> ReadPageAsync(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)) throw new ArgumentException(nameof(url));
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpService.SendAsync(httpRequestMessage);

            if (!response.IsSuccessStatusCode) throw new FailedRequestException(response.StatusCode);

            var items = await _deserializer.DeserializeAsync<ImmutableList<TItem>>(response.Content);
            response.Headers.TryGetValues("Link", out var links);
            return new CollectionPageData<TItem>(items, links.FirstOrDefault());
        }
    }
}
