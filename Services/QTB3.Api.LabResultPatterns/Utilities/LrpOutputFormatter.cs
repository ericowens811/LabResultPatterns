using System.Buffers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using QTB3.Api.Abstractions.Utilities;

namespace QTB3.Api.LabResultPatterns.Utilities
{
    public class LrpOutputFormatter : JsonOutputFormatter
    {
        public LrpOutputFormatter(ISupportedMedia lrpSupportedMedia, JsonSerializerSettings serializerSettings, ArrayPool<char> charPool) : base(serializerSettings, charPool)
        {
            SupportedMediaTypes.Clear();
            foreach (var type in lrpSupportedMedia.Types)
            {
                SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(type));
            }
        }
    }
}
