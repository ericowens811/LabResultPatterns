using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using QTB3.Client.Abstractions.Services.Serialization;

namespace QTB3.Client.Common.Services.Serialization
{
    public class JsonContentSerializer : IContentSerializer
    {
        public void Serialize<T>(HttpRequestMessage message, T item)
        {
            if(message == null) throw new ArgumentNullException(nameof(message));
            if(item == null) throw new ArgumentNullException(nameof(item));
            var postJsonString = JsonConvert.SerializeObject(item);
            message.Content = new StringContent(postJsonString, Encoding.UTF8, "application/json");
        }
    }
}
