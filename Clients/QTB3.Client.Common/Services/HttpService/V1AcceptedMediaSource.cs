using System.Net.Http.Headers;
using QTB3.Client.Abstractions.Services.HttpService;

namespace QTB3.Client.Common.Services.HttpService
{
    public class V1AcceptedMediaSource : IAcceptedMediaSource
    {
        public const string LrpMediaTypeV1 = "application/vnd.lrp.v1+json";
        public MediaTypeWithQualityHeaderValue Get()
        {
            return new MediaTypeWithQualityHeaderValue(LrpMediaTypeV1);
        }
    }
}
