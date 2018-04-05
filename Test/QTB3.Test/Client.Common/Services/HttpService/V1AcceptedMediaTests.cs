using System.Net.Http.Headers;
using NUnit.Framework;
using QTB3.Client.Common.Services.HttpService;

namespace QTB3.Test.Client.Common.Services.HttpService
{
    [TestFixture]
    public class V1AcceptedMediaTests
    {
        [Test]
        [Category("V1AcceptedMediaTests")]
        public void Get()
        {
            var uut = new V1AcceptedMediaSource();
            var expected = new MediaTypeWithQualityHeaderValue("application/vnd.lrp.v1+json");
            Assert.AreEqual(expected, uut.Get());
        }
    }
}
