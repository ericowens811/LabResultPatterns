using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using QTB3.Client.Common.Services.Serialization;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.LabResultPatterns.Support.ModelEquality;

namespace QTB3.Test.Client.Common.Services.Serialization
{
    [TestFixture]
    public class JsonContentDeserializerTests
    {
        [Test]
        [Category("JsonContentDeserializer")]
        public async Task DeserializeThrows()
        {
            var deserializer = new JsonContentDeserializer();
            Assert.ThrowsAsync<ArgumentNullException>
            (
                async () => await deserializer.DeserializeAsync<Uom>(null)
            );
        }

        [Test]
        [Category("JsonContentDeserializer")]
        public async Task DeserializeSucceeds()
        {
            var expectedUom = new Uom().WithId(1).WithName("Uom").WithDescription("The Uom"); 
            var uomJson = JsonConvert.SerializeObject(expectedUom);
            var httpContent = new StringContent(uomJson);
            var deserializer = new JsonContentDeserializer();
            var actualUom = await deserializer.DeserializeAsync<Uom>(httpContent);
            Assert.True(UomEqual.Check(expectedUom, actualUom));
        }
    }
}
