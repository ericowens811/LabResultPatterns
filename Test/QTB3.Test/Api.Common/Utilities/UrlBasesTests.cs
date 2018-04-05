using System;
using NUnit.Framework;
using QTB3.Api.Common.Utilities;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Api.Common.Utilities
{
    [TestFixture]
    public class UrlBasesTests
    {
        [Test]
        [Category("UrlBases")]
        public void Contructor()
        {
            ConstructorTests<UrlBases>
                .For(typeof(string), typeof(string))
                .Fail(new object[] {"http://local", ""}, typeof(ArgumentException), "Bad Write Url")
                .Fail(new object[] {"", "http://local" }, typeof(ArgumentException), "Bad Read Url")
                .Fail(new object[] {"", ""}, typeof(ArgumentException), "Bad Read and Write Urls")
                .Succeed(new object[] { "http://local", "http://local" }, "Constructor args valid")
                .Assert();
        }

    }
}
