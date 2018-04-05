
using System;
using NUnit.Framework;
using QTB3.Client.LabResultPatterns.Common.Linking;
using QTB3.Model.Abstractions;

namespace QTB3.Test.Client.LabResultPatterns.Common.Linking
{
    [TestFixture]
    public class LinksTests
    {
        [Test]
        [Category("Link")]
        public void MissingKeyOnGet()
        {
            var links = new Links();
            Assert.Throws<ArgumentException>(() => links.GetUrl(RelTypes.self));
        }

        [Test]
        [Category("Link")]
        public void KeyReturnsUrl()
        {
            var expectedUrl = "https://localhost:8080/a/b";
            var links = new Links();
            links.SetUrl(RelTypes.first, expectedUrl);
            var actualUrl = links.GetUrl(RelTypes.first);
            Assert.AreEqual(expectedUrl, actualUrl);
        }

        [Test]
        [Category("Link")]
        public void RejectsForNotFound()
        {
            var validUrl = "https://localhost:8080/a/b";
            var links = new Links();
            Assert.Throws<ArgumentOutOfRangeException>(() => links.SetUrl(RelTypes.notfound, validUrl));
        }

        [Test]
        [Category("Link")]
        public void RejectsForBadUrl()
        {
            var invalidUrl = "://localhost//";
            var links = new Links();
            Assert.Throws<ArgumentException>(() => links.SetUrl(RelTypes.self, invalidUrl));
        }
    }
}
