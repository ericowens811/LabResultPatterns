using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using NUnit.Framework;
using QTB3.Api.Abstractions.Utilities;
using QTB3.Api.Common.Utilities;
using QTB3.Model.LabResultPatterns.Paging;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Api.Common.Utilities
{
    [TestFixture]
    public class PageLinksBuilderTests 
    { 
        [Test]
        [Category("PageLinksBuilder")]
        public void Contructor()
        { 
            var urlBaseBuilder = new Mock<IUrlBaseBuilder>().Object;
            var pageLinksFormatter = new Mock<IPageLinksFormatter>().Object;
            ConstructorTests<PageLinksBuilder>
                .For(typeof(IUrlBaseBuilder), typeof(IPageLinksFormatter))
                .Fail(new object[] { null, pageLinksFormatter }, typeof(ArgumentNullException), "Null UrlBaseBuilder")
                .Fail(new object[] { urlBaseBuilder, null }, typeof(ArgumentNullException), "Null PageLinksFormatter")
                .Succeed(new object[] { urlBaseBuilder, pageLinksFormatter }, "Constructor args valid")
                .Assert();
        }

        [Test]
        [Category("PageLinksBuilder")]
        public void BuildHttpRequestNull()
        {
            var urlBaseBuilder = new Mock<IUrlBaseBuilder>(MockBehavior.Strict).Object;
            var pageLinksFormatter = new Mock<IPageLinksFormatter>(MockBehavior.Strict).Object;
            var uut = new PageLinksBuilder(urlBaseBuilder, pageLinksFormatter);
            Assert.Throws<ArgumentNullException>
            (
                () => uut.Build(new Page<Uom>(), null)
            );
        }

        [Test]
        [Category("PageLinksBuilder")]
        public void BuildPageNull()
        {
            var httpRequest = new DefaultHttpRequest(new DefaultHttpContext());
            var urlBaseBuilder = new Mock<IUrlBaseBuilder>(MockBehavior.Strict).Object;
            var pageLinksFormatter = new Mock<IPageLinksFormatter>(MockBehavior.Strict).Object;
            var uut = new PageLinksBuilder(urlBaseBuilder, pageLinksFormatter);
            Assert.Throws<ArgumentNullException>
            (
                () => uut.Build<Uom>(null, httpRequest)
            );
        }

        [Test]
        [Category("PageLinksBuilder")]
        public void BuildSucceeds()
        {
            var page = new Page<Uom>();
            var httpRequest = new DefaultHttpRequest(new DefaultHttpContext());
            var urlBases = new UrlBases("http://localhost/acid/base", "http://localhost");
            var urlBaseBuilder = new Mock<IUrlBaseBuilder>(MockBehavior.Strict);
            urlBaseBuilder.Setup(u => u.Build(httpRequest)).Returns(urlBases);
            var expectedLinks = "theLinks";
            var pageLinksFormatter = new Mock<IPageLinksFormatter>(MockBehavior.Strict);
            pageLinksFormatter.Setup(f => f.GetLinks(urlBases.ReadUrl, page)).Returns(expectedLinks);
            var uut = new PageLinksBuilder(urlBaseBuilder.Object, pageLinksFormatter.Object);
            var actualLinks = uut.Build(page, httpRequest);
            Assert.AreEqual(expectedLinks, actualLinks);
        }

    }
}
