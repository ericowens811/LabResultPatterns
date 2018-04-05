using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Moq;
using NUnit.Framework;
using QTB3.Api.Common.Utilities;
using QTB3.Client.Common.Services.Data;
using QTB3.Model.Abstractions;
using QTB3.Model.LabResultPatterns.Paging;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.LabResultPatterns.ClientModel.Pagebar;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Client.Common.Services.Data
{
    [TestFixture]
    public class CollectionPageDataTests
    {
        public const string Url = "http://localhost/acid/base";

        [Test]
        [Category("CollectionPageData")]
        public void Constructor()
        {
            var items = new Mock<IImmutableList<Uom>>().Object;
            var links = new PageLinksFormatter().GetLinks(Url, new Page<Uom>("", 40, 0, 10, new List<Uom>()));

            ConstructorTests<CollectionPageData<Uom>>
                .For(typeof(IImmutableList<Uom>), typeof(string))
                .Fail(new object[] {items, null}, typeof(ArgumentNullException), "Null links")
                .Fail(new object[] {null, links}, typeof(ArgumentNullException), "Null items")
                .Succeed(new object[] {items, links}, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("CollectionPageData")]
        public void NotPaging()
        {
            var items = new List<Uom>().ToImmutableList();
            var links = new PageLinksFormatter().GetLinks(Url, new Page<Uom>("", 40, 0, 50, new List<Uom>()));
            var data = new CollectionPageData<Uom>(items, links);

            data.Links.TryGetValue(RelTypes.self, out var value);

            Assert.IsFalse(data.IsPaging);
            Assert.AreEqual(1, data.Links.Count);
            Assert.NotNull(value);
            Assert.AreEqual($"{Url}?searchText=&skip=0&take=50", value);
        }

        [Test]
        [Category("CollectionPageData")]
        public void OnFirstPage()
        {
            var items = new List<Uom>().ToImmutableList();
            var links = new PageLinksFormatter().GetLinks(Url, new Page<Uom>("", 200, 0, 50, new List<Uom>()));
            var expectedPagingText = PagingTextCalculator.Calculate(0, 50, 200);
            var data = new CollectionPageData<Uom>(items, links);

            Assert.IsTrue(data.IsPaging);
            Assert.AreEqual(4, data.Links.Count);

            data.Links.TryGetValue(RelTypes.self, out var selfUrl);
            data.Links.TryGetValue(RelTypes.first, out var firstUrl);
            data.Links.TryGetValue(RelTypes.next, out var nextUrl);
            data.Links.TryGetValue(RelTypes.last, out var lastUrl);
            Assert.AreEqual($"{Url}?searchText=&skip=0&take=50", selfUrl);
            Assert.AreEqual($"{Url}?searchText=&skip=0&take=50", firstUrl);
            Assert.AreEqual($"{Url}?searchText=&skip=50&take=50", nextUrl);
            Assert.AreEqual($"{Url}?searchText=&skip=150&take=50", lastUrl);

            Assert.AreEqual(expectedPagingText, data.PagingText);
        }

        [Test]
        [Category("CollectionPageData")]
        public void OnMiddlePage()
        {
            var items = new List<Uom>().ToImmutableList();
            var links = new PageLinksFormatter().GetLinks(Url, new Page<Uom>("", 200, 50, 50, new List<Uom>()));
            var expectedPagingText = PagingTextCalculator.Calculate(50, 50, 200);
            var data = new CollectionPageData<Uom>(items, links);

            Assert.IsTrue(data.IsPaging);
            Assert.AreEqual(5, data.Links.Count);

            data.Links.TryGetValue(RelTypes.self, out var selfUrl);
            data.Links.TryGetValue(RelTypes.first, out var firstUrl);
            data.Links.TryGetValue(RelTypes.next, out var nextUrl);
            data.Links.TryGetValue(RelTypes.prev, out var prevUrl);
            data.Links.TryGetValue(RelTypes.last, out var lastUrl);
            Assert.AreEqual($"{Url}?searchText=&skip=50&take=50", selfUrl);
            Assert.AreEqual($"{Url}?searchText=&skip=0&take=50", firstUrl);
            Assert.AreEqual($"{Url}?searchText=&skip=100&take=50", nextUrl);
            Assert.AreEqual($"{Url}?searchText=&skip=0&take=50", prevUrl);
            Assert.AreEqual($"{Url}?searchText=&skip=150&take=50", lastUrl);

            Assert.AreEqual(expectedPagingText, data.PagingText);
        }

        [Test]
        [Category("CollectionPageData")]
        public void OnLastPage()
        {
            var items = new List<Uom>().ToImmutableList();
            var links = new PageLinksFormatter().GetLinks(Url, new Page<Uom>("", 200, 150, 50, new List<Uom>()));
            var expectedPagingText = PagingTextCalculator.Calculate(150, 50, 200);
            var data = new CollectionPageData<Uom>(items, links);

            Assert.IsTrue(data.IsPaging);
            Assert.AreEqual(4, data.Links.Count);

            data.Links.TryGetValue(RelTypes.self, out var selfUrl);
            data.Links.TryGetValue(RelTypes.first, out var firstUrl);
            data.Links.TryGetValue(RelTypes.prev, out var prevUrl);
            data.Links.TryGetValue(RelTypes.last, out var lastUrl);
            Assert.AreEqual($"{Url}?searchText=&skip=150&take=50", selfUrl);
            Assert.AreEqual($"{Url}?searchText=&skip=0&take=50", firstUrl);
            Assert.AreEqual($"{Url}?searchText=&skip=100&take=50", prevUrl);
            Assert.AreEqual($"{Url}?searchText=&skip=150&take=50", lastUrl);

            Assert.AreEqual(expectedPagingText, data.PagingText);
        }
    }
}
