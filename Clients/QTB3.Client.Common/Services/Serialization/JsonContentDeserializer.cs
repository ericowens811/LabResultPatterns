using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QTB3.Client.Abstractions.Services.Serialization;

namespace QTB3.Client.Common.Services.Serialization
{
    public class JsonContentDeserializer : IContentDeserializer
    {
        public async Task<T> DeserializeAsync<T>(HttpContent content)
        {
            if(content == null) throw new ArgumentNullException(nameof(content));
            var jsonString = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
