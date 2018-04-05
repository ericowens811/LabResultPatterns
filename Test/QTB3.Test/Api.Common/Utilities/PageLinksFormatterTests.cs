using System.Collections.Generic;
using NUnit.Framework;
using QTB3.Api.Common.Utilities;
using QTB3.Model.LabResultPatterns.Paging;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.LabResultPatterns.Support.Links;

namespace QTB3.Test.Api.Common.Utilities
{
    [TestFixture]
    public class PageLinksFormatterTests : LinkTestsBase
    {
        [Test]
        [Category("PageLinksFormatter")]
        public void BuildOnFirstPageOfMany()
        {
            var links = new PageLinksFormatter().GetLinks("http://localhost/acid/base", new Page<Uom>("", 40, 0, 10, new List<Uom>()));
            var linksArray = links.Split(',');
            Assert.AreEqual(4, linksArray.Length);
            Assert.AreEqual("http://localhost/acid/base?searchText=&skip=0&take=10", GetUrlString(linksArray, "self"));
            Assert.AreEqual("http://localhost/acid/base?searchText=&skip=0&take=10", GetUrlString(linksArray, "first"));
            Assert.AreEqual("http://localhost/acid/base?searchText=&skip=10&take=10", GetUrlString(linksArray, "next"));
            Assert.AreEqual("http://localhost/acid/base?searchText=&skip=30&take=10", GetUrlString(linksArray, "last"));
        }

        [Test]
        [Category("PageLinksFormatter")]
        public void BuildOnSecondPageOfMany()
        {
            var links = new PageLinksFormatter().GetLinks("http://localhost/acid/base", new Page<Uom>("", 40, 10, 10, new List<Uom>()));
            var linksArray = links.Split(',');
            Assert.AreEqual(5, linksArray.Length);
            Assert.AreEqual("http://localhost/acid/base?searchText=&skip=10&take=10", GetUrlString(linksArray, "self"));
            Assert.AreEqual("http://localhost/acid/base?searchText=&skip=0&take=10", GetUrlString(linksArray, "first"));
            Assert.AreEqual("http://localhost/acid/base?searchText=&skip=20&take=10", GetUrlString(linksArray, "next"));
            Assert.AreEqual("http://localhost/acid/base?searchText=&skip=0&take=10", GetUrlString(linksArray, "prev"));
            Assert.AreEqual("http://localhost/acid/base?searchText=&skip=30&take=10", GetUrlString(linksArray, "last"));
        }

        [Test]
        [Category("PageLinksFormatter")]
        public void BuildOnLastPageOfMany()
        {
            var links = new PageLinksFormatter().GetLinks("http://localhost/acid/base", new Page<Uom>("", 40, 30, 10, new List<Uom>()));
            var linksArray = links.Split(',');
            Assert.AreEqual(4, linksArray.Length);
            Assert.AreEqual("http://localhost/acid/base?searchText=&skip=30&take=10", GetUrlString(linksArray, "self"));
            Assert.AreEqual("http://localhost/acid/base?searchText=&skip=0&take=10", GetUrlString(linksArray, "first"));
            Assert.AreEqual("http://localhost/acid/base?searchText=&skip=20&take=10", GetUrlString(linksArray, "prev"));
            Assert.AreEqual("http://localhost/acid/base?searchText=&skip=30&take=10", GetUrlString(linksArray, "last"));
        }

        [Test]
        [Category("PageLinksFormatter")]
        public void BuildOnSecondPageOfManySearchText()
        {
            var links = new PageLinksFormatter().GetLinks("http://localhost/acid/base", new Page<Uom>("j", 40, 10, 10, new List<Uom>()));
            var linksArray = links.Split(',');
            Assert.AreEqual(5, linksArray.Length);
            Assert.AreEqual("http://localhost/acid/base?searchText=j&skip=10&take=10", GetUrlString(linksArray, "self"));
            Assert.AreEqual("http://localhost/acid/base?searchText=j&skip=0&take=10", GetUrlString(linksArray, "first"));
            Assert.AreEqual("http://localhost/acid/base?searchText=j&skip=20&take=10", GetUrlString(linksArray, "next"));
            Assert.AreEqual("http://localhost/acid/base?searchText=j&skip=0&take=10", GetUrlString(linksArray, "prev"));
            Assert.AreEqual("http://localhost/acid/base?searchText=j&skip=30&take=10", GetUrlString(linksArray, "last"));
        }

    }
}
