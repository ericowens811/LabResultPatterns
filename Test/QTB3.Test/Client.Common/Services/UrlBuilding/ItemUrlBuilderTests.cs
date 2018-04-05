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
    public class ItemUrlBuilderTests
    {
        public string Key; 
        public LinkTemplateLookup Lookup; 
            

        [SetUp]
        public void Setup()
        {
            Key = "key";
            Lookup = new LinkTemplateLookup() {["key"] = "http://qtb3.com/uom/{id}"};
        }


        [Test]
        [Category("ItemUrlBuilder")]
        public void Constructor()
        {
            var templateKey = "key";
            var lookup = new Mock<ILinkTemplateLookup>().Object;
            ConstructorTests<ItemUrlBuilder<Uom>>
                .For(typeof(string), typeof(ILinkTemplateLookup))
                .Fail(new object[] { null, lookup }, typeof(ArgumentException), "Null templateKey")
                .Fail(new object[] { templateKey, null }, typeof(ArgumentNullException), "Null templateKey")
                .Succeed(new object[] { templateKey, lookup }, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("ItemUrlBuilder")]
        public void Build_Id0()
        {
            var uut = new ItemUrlBuilder<Uom>(Key, Lookup);
            Assert.Throws<ArgumentOutOfRangeException>(
                () => uut.Build(0)
            );
        }

        [Test]
        [Category("ItemUrlBuilder")]
        public void Build_Id1001()
        {
            var uut = new ItemUrlBuilder<Uom>(Key, Lookup);
            var value = uut.Build(1001);
            Assert.AreEqual("http://qtb3.com/uom/1001", value);
        }

        [Test]
        [Category("ItemUrlBuilder")]
        public void Build()
        {
            var uut = new ItemUrlBuilder<Uom>(Key, Lookup);
            var value = uut.Build();
            Assert.AreEqual("http://qtb3.com/uom", value);
        }
    }
}
