using System;
using NUnit.Framework;
using QTB3.Client.Common.Services.Validation;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;

namespace QTB3.Test.Client.Common.Services.Validation
{
    [TestFixture]
    public class ValidatorTests
    {
        [Test]
        [Category("Validator")]
        public void NullArgument()
        {
            var validator = new Validator();
            Assert.Throws<ArgumentNullException>
            (
                () => validator.Validate(null, out _)
            );
        }

        [Test]
        [Category("Validator")]
        public void ValidReturnsTrue()
        {
            var uom = new Uom(0, "TheUom", "AllOfIt");
            Assert.IsTrue(new Validator().Validate(uom, out _));
        }

        [Test]
        [Category("Validator")]
        public void InvalidOneErrorInDictionary()
        {
            var uom = new Uom(0, "", "AllOfIt");
            Assert.IsFalse(new Validator().Validate(uom, out var errorDictionary));
            Assert.AreEqual(1, errorDictionary.Count);
        }

        [Test]
        [Category("Validator")]
        public void InvalidTwoErrorsInDictionary()
        {
            var uom = new Uom();
            new Validator().Validate(uom, out var errorDictionary);
            Assert.AreEqual(2, errorDictionary.Count);
        }

        [Test]
        [Category("Validator")]
        public void InvalidHasValueForName()
        {
            var uom = new Uom(0, "", "AllOfIt");
            new Validator().Validate(uom, out var errorDictionary);
            errorDictionary.TryGetValue("Name", out var value);
            Assert.NotNull(value);
            Assert.AreEqual(1, value?.Count);
            Assert.AreEqual("The Name field is required.", value?[0]);
        }

        [Test]
        [Category("Validator")]
        public void InvalidHasValueForDescription()
        {
            var uom = new Uom(0, "TheUom", "");
            new Validator().Validate(uom, out var errorDictionary);
            errorDictionary.TryGetValue("Description", out var value);
            Assert.NotNull(value);
            Assert.AreEqual(1, value?.Count);
            Assert.AreEqual("The Description field is required.", value?[0]);
        }

        [Test]
        [Category("Validator")]
        public void InvalidHasValueForNameAndDescription()
        {
            var uom = new Uom(0, "", "");
            new Validator().Validate(uom, out var errorDictionary);
            errorDictionary.TryGetValue("Description", out var descriptionValue);
            Assert.NotNull(descriptionValue);
            Assert.AreEqual(1, descriptionValue?.Count);
            Assert.AreEqual("The Description field is required.", descriptionValue?[0]);
            errorDictionary.TryGetValue("Name", out var nameValue);
            Assert.NotNull(nameValue);
            Assert.AreEqual(1, nameValue?.Count);
            Assert.AreEqual("The Name field is required.", nameValue?[0]);
        }
    }
}
