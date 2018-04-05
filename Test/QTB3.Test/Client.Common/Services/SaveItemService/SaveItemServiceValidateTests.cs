using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Services.SaveItemService;
using QTB3.Client.Abstractions.Services.Validation;
using QTB3.Client.Common.Services.Exceptions;
using QTB3.Client.Common.Services.SaveItemService;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Client.Common.Services.SaveItemService
{
    [TestFixture]
    public class SaveItemServiceValidateTests
    {
        [Test]
        [Category("SaveItemServiceValidate")]
        public void Constructor()
        {
            var validator = new Mock<IValidator>().Object;
            var next = new Mock<ISaveItemService<Uom>>().Object;
            ConstructorTests<SaveItemServiceValidate<Uom>>
                .For(typeof(IValidator), typeof(ISaveItemService<Uom>))
                .Fail(new object[] { null, next }, typeof(ArgumentNullException), "Null validator")
                .Fail(new object[] { validator, null }, typeof(ArgumentNullException), "Null next")
                .Succeed(new object[] { validator, next }, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("SaveItemServiceValidate")]
        public void SaveItemAsync_NullItem()
        {
            var validator = new Mock<IValidator>(MockBehavior.Strict).Object;
            var next = new Mock<ISaveItemService<Uom>>(MockBehavior.Strict).Object;
            var uut = new SaveItemServiceValidate<Uom>(validator, next);
            Assert.ThrowsAsync<ArgumentNullException>
            (
                async () => await uut.SaveItemAsync(null)
            );
        }

        [Test]
        [Category("SaveItemServiceValidate")]
        public void SaveItemAsync_InvalidItem()
        {
            var uom = new Uom();
            var expectedErrorDictionary = new Dictionary<string, ReadOnlyCollection<string>>();
            var validator = new Mock<IValidator>(MockBehavior.Strict);
            validator.Setup(v => v.Validate(uom, out expectedErrorDictionary)).Returns(false);
            var next = new Mock<ISaveItemService<Uom>>(MockBehavior.Strict).Object;
            var uut = new SaveItemServiceValidate<Uom>(validator.Object, next);
            var exception = Assert.ThrowsAsync<BadRequestHttpException>
            (
                async () => await uut.SaveItemAsync(uom)
            );
            Assert.AreEqual(expectedErrorDictionary, exception.ErrorDictionary);
        }

        [Test]
        [Category("SaveItemServiceValidate")]
        public async Task SaveItemAsync()
        {
            var uom = new Uom();
            //var expectedErrorDictionary = new Dictionary<string, ReadOnlyCollection<string>>();
            Dictionary<string, ReadOnlyCollection<string>> expectedErrorDictionary = null;
            var validator = new Mock<IValidator>(MockBehavior.Strict);
            validator.Setup(v => v.Validate(uom, out expectedErrorDictionary)).Returns(true);
            var next = new Mock<ISaveItemService<Uom>>(MockBehavior.Strict);
            next.Setup(n => n.SaveItemAsync(uom)).Returns(Task.CompletedTask);
            var uut = new SaveItemServiceValidate<Uom>(validator.Object, next.Object);
            await uut.SaveItemAsync(uom);
        }
    }
}
