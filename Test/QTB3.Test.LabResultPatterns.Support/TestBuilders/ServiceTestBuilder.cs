using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using Moq;
using QTB3.Client.Abstractions.Paging;
using QTB3.Client.Abstractions.Services;
using QTB3.Client.Abstractions.Services.Configuration;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Client.Abstractions.Services.Validation;
using QTB3.Client.Common.Services;
using QTB3.Client.Common.Services.LinkService;
using QTB3.Client.LabResultPatterns.Common.Configuration;
using QTB3.Model.Abstractions;

namespace QTB3.Test.LabResultPatterns.Support.TestBuilders
{
    public class ServiceTestBuilder<TItem> where TItem : class, IEntity
    {
    
        // --------------------------------------------------
        //
        // UUT components
        //
        // --------------------------------------------------
        protected IValidator Validator;

        protected IHttpRequestBuilder HttpRequestBuilder;
        protected IEndPoint ApiEndPoint;
        protected IHttpReadClient HttpClient;

        // --------------------------------------------------
        //
        // Builder output
        //
        // --------------------------------------------------


        public static implicit operator LinkService(ServiceTestBuilder<TItem> builder)
        {
            return new LinkService
            (
                builder.ApiEndPoint,
                builder.HttpRequestBuilder,
                builder.HttpClient
            );
        }


        // --------------------------------------------------
        //
        // HttpRequestBuilder setup
        //
        // --------------------------------------------------
        public ServiceTestBuilder<TItem> HttpRequestBuilder_BuildAsync
        (
            HttpMethod method, 
            string url, 
            HttpRequestMessage request
        )
        {
            var creator = new Mock<IHttpRequestBuilder>(MockBehavior.Strict);
            creator.Setup(b => b.BuildAsync(method, url)).ReturnsAsync(request);
            HttpRequestBuilder = creator.Object;
            return this;
        }


        // --------------------------------------------------
        //
        // HttpClient setup
        //
        // --------------------------------------------------
        public ServiceTestBuilder<TItem> HttpClient_NotCalled()
        {
            HttpClient = new Mock<IHttpReadClient>(MockBehavior.Strict).Object;
            return this;
        }

        public ServiceTestBuilder<TItem> HttpClient_SendAsync(HttpRequestMessage request, HttpResponseMessage response)
        {
            var creator = new Mock<IHttpReadClient>(MockBehavior.Strict);
            creator.Setup(c => c.SendAsync(request)).ReturnsAsync(response);
            HttpClient = creator.Object;
            return this;
        }

        // --------------------------------------------------
        //
        // ApiEndPoint setup
        //
        // --------------------------------------------------
        public ServiceTestBuilder<TItem> ApiEndPoint_GetUrl(string url)
        {
            var creator = new Mock<IEndPoint>(MockBehavior.Strict);
            creator
                .Setup(d => d.Url)
                .Returns(url);
            ApiEndPoint = creator.Object;
            return this;
        }
    }
}
