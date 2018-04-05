using System;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using QTB3.Client.Common.Services.Serialization;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.LabResultPatterns.Support.ModelEquality;

namespace QTB3.Test.Client.Common.Services.Serialization
{
    [TestFixture]
    public class JsonContentSerializerTests
    {
        [Test]
        [Category("JsonContentSerializer")]
        public void SerializeNullMessageThrows()
        {
            var uom = new Uom();
            var serializer = new JsonContentSerializer();
            Assert.Throws<ArgumentNullException>
            (
                () => serializer.Serialize<Uom>(null, uom)
            );
        }

        [Test]
        [Category("JsonContentSerializer")]
        public void SerializeNullItemThrows()
        {
            var message = new HttpRequestMessage();
            var serializer = new JsonContentSerializer();
            Assert.Throws<ArgumentNullException>
            (
                () => serializer.Serialize<Uom>(message, null)
            );
        }

        [Test]
        [Category("JsonContentSerializer")]
        public async Task SerializeSucceeds()
        {
            var expectedItem = new Uom(1, "TheUom", "TheDescription");
            var message = new HttpRequestMessage();
            var serializer = new JsonContentSerializer();
            serializer.Serialize<Uom>(message, expectedItem);
            var deserializer = new JsonContentDeserializer();
            var actualItem = await deserializer.DeserializeAsync<Uom>(message.Content);
            Assert.True(UomEqual.Check(expectedItem, actualItem));
        }

    }
}
