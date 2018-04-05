using System;
using System.Net.Http;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Client.Abstractions.Services.ReadItemService;
using QTB3.Client.Abstractions.Services.Serialization;
using QTB3.Client.Common.Services.Exceptions;
using IHttpReadService = QTB3.Client.Abstractions.Services.HttpService.IHttpReadService;

namespace QTB3.Client.Common.Services.ReadItemService
{
    public class ReadItemServiceBuildRequest<TItem> : IReadItemServiceBuildRequest<TItem>
    {
        private readonly IHttpService _httpService;
        private readonly IContentDeserializer _deserializer;

        public ReadItemServiceBuildRequest
        (
            IHttpReadService httpService,
            IContentDeserializer deserializer
        )
        {
            _httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
            _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
        }

        public async Task<TItem> ReadItemAsync(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)) throw new ArgumentException(nameof(url));
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpService.SendAsync(httpRequestMessage);

            if (!response.IsSuccessStatusCode) throw new FailedRequestException(response.StatusCode);

            return await _deserializer.DeserializeAsync<TItem>(response.Content);
        }
    }
}
