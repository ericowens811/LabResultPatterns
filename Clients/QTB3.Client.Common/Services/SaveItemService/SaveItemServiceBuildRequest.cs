using System;
using System.Net.Http;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Client.Abstractions.Services.SaveItemService;
using QTB3.Client.Abstractions.Services.Serialization;
using QTB3.Client.Common.Services.Exceptions;
using QTB3.Model.Abstractions;
using IHttpWriteService = QTB3.Client.Abstractions.Services.HttpService.IHttpWriteService;

namespace QTB3.Client.Common.Services.SaveItemService
{
    public class SaveItemServiceBuildRequest<TItem> : ISaveItemServiceBuildRequest<TItem>
        where TItem : class, IEntity
    {
        private readonly IHttpService _httpService;
        private readonly IContentSerializer _contentSerializer;

        public SaveItemServiceBuildRequest
        (
            IHttpWriteService httpService,
            IContentSerializer contentSerializer
        )
        {
            _httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
            _contentSerializer = contentSerializer ?? throw new ArgumentNullException(nameof(contentSerializer));
        }

        public async Task SaveItemAsync(string url, TItem item)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)) throw new ArgumentException(nameof(url));
            if (item == null) throw new ArgumentNullException(nameof(item));
            var httpRequestMessage = item.Id == 0
                ? new HttpRequestMessage(HttpMethod.Post, url)
                : new HttpRequestMessage(HttpMethod.Put, url);
            _contentSerializer.Serialize(httpRequestMessage, item);

            var response = await _httpService.SendAsync(httpRequestMessage);
            // while its possible that the server's implementation of validation
            // could return errors that escaped the client's implementation of validation,
            // we choose not to decode any error dictionary returned by the server.
            // We validate on the server to protect against clients that fail to
            // validate as this class does
            if (!response.IsSuccessStatusCode)
            {
                throw new FailedRequestException(response.StatusCode);
            }
        }
    }
}
