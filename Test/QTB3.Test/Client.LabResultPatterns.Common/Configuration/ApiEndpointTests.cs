
using System;
using NUnit.Framework;
using QTB3.Client.LabResultPatterns.Common.Configuration;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Client.LabResultPatterns.Common.Configuration
{
    [TestFixture]
    public class ApiEndpointTests
    {

        public const string Url = "http://a.com/b";

        [Test]
        [Category("ApiEndpoint")]
        public void Constructor()
        {
            var goodUrl = Url;
            var badUrl = "asdf";

            ConstructorTests<ApiEndpoint>
                .For(typeof(string))
                .Fail(new object[] {badUrl}, typeof(ArgumentException), "Bad url")
                .Succeed(new object[] {goodUrl}, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("ApiEndpoint")]
        public void Getter()
        {
            var apiEndpoint = new ApiEndpoint(Url);
            Assert.AreEqual(Url, apiEndpoint.Url);
        }

    }
}
