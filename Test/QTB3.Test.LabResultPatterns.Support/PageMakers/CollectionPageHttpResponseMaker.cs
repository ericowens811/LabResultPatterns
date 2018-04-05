
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using QTB3.Api.Common.Utilities;
using QTB3.Model.Abstractions;
using QTB3.Model.LabResultPatterns.Paging;

namespace QTB3.Test.LabResultPatterns.Support.PageMakers
{
    public static class CollectionPageHttpResponseMaker
    {
        public static HttpResponseMessage GetExpectedResponse<T>
        (
            Page<T> expectedPage,
            string baseUrl
        )
        where T: class, IEntity
        {
            var links = new PageLinksFormatter().GetLinks(baseUrl, expectedPage);
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(expectedPage.Items))
            };
            response.Headers.Add("Link", links);
            return response;
        }
    }
}
