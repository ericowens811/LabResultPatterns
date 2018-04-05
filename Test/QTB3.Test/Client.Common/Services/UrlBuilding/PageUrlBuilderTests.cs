using System;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Services.UrlBuilding;
using QTB3.Client.Common.Services.UrlBuilding;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Client.Common.Services.UrlBuilding
{
    [TestFixture]
    public class PageUrlBuilderTests
    {
        public string Key;
        public LinkTemplateLookup Lookup;
        public int PageSize;
        public int Skip = 0;

        [SetUp]
        public void Setup()
        {
            Key = "key";
            Lookup = new LinkTemplateLookup() {["key"] = "http://qtb3.com/uom?searchText={filter}&skip={skip}&take={take}"};
            PageSize = 20;
        }

        [Test]
        [Category("PageUrlBuilder")]
        public void Constructor()
        {
            var pageSize = 20;
            var templateKey = "key";
            var lookup = new Mock<ILinkTemplateLookup>().Object;
            ConstructorTests<PageUrlBuilder<Uom>>
                .For(typeof(string), typeof(ILinkTemplateLookup), typeof(int))
                .Fail(new object[] {null, lookup, pageSize}, typeof(ArgumentException), "Null templateKey")
                .Fail(new object[] {templateKey, null, pageSize}, typeof(ArgumentNullException), "Null templateKey")
                .Fail(new object[] {templateKey, lookup, 0}, typeof(ArgumentException), "Invalid pageSize")
                .Succeed(new object[] {templateKey, lookup, pageSize}, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("PageUrlBuilder")]
        public void Build_Filter()
        {
            var filter = "zzz";
            var uut = new PageUrlBuilder<Uom>(Key, Lookup, PageSize);
            var url = uut.Build(filter);
            Assert.AreEqual($"http://qtb3.com/uom?searchText={filter}&skip={Skip}&take={PageSize}", url);
        }

        [Test]
        [Category("PageUrlBuilder")]
        public void Build()
        {
            var uut = new PageUrlBuilder<Uom>(Key, Lookup, PageSize);
            var url = uut.Build(null);
            Assert.AreEqual($"http://qtb3.com/uom?searchText=&skip={Skip}&take={PageSize}", url);
        }
    }
}
