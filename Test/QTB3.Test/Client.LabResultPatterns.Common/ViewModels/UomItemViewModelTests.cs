using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Services;
using QTB3.Client.Abstractions.Services.ReadItemService;
using QTB3.Client.Abstractions.Services.SaveItemService;
using QTB3.Client.Common.Services.Exceptions;
using QTB3.Client.LabResultPatterns.Common.Constants;
using QTB3.Client.LabResultPatterns.Common.UomComponents;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.LabResultPatterns.Support.ModelEquality;
using QTB3.Test.LabResultPatterns.Support.TestBuilders;
using QTB3.Test.Support.ConstructorTesting;

namespace QTB3.Test.Client.LabResultPatterns.Common.ViewModels
{
    [TestFixture]
    public class UomItemViewModelTests 
    {
        [Test]
        [Category("UomItemViewModel")]
        public void Constructor()
        {
            var saveService = new Mock<ISaveItemService<Uom>>().Object;
            var readService = new Mock<IReadItemService<Uom>>().Object;
            ConstructorTests<UomItemViewModel>
                .For(typeof(ISaveItemService<Uom>), typeof(IReadItemService<Uom>))
                .Fail(new object[] { null, readService }, typeof(ArgumentNullException), "Null saveService.")
                .Fail(new object[] { saveService, null }, typeof(ArgumentNullException), "Null readService.")
                .Succeed(new object[] { saveService, readService }, "Constructor args valid.")
                .Assert();
        }

        [Test]
        [Category("UomItemViewModel")]
        public async Task GetItemAsync_id0()
        {
            UomItemViewModel viewModel = new ViewModelTestBuilder<Uom>()
                .ReadItemService_NotCalled()
                .SaveItemService_NotCalled();
            await viewModel.GetItemAsync(0);
            Assert.True(UomItemViewModelEqual.Check
            (
                expected: new Uom(),
                expectedNameErrors: null,
                expectedDescriptionErrors: null,
                actual: viewModel
            ));
        }

        [Test]
        [Category("UomItemViewModel")]
        public async Task GetItemAsync_IdNonZero()
        {
            var expectedUom = new Uom(1001, "in", "inch");
            UomItemViewModel viewModel = new ViewModelTestBuilder<Uom>()
                .SaveItemService_NotCalled()
                .Then_ReadItemService_ReadItemAsync(expectedUom);
            await viewModel.GetItemAsync(1001);
            Assert.True(UomItemViewModelEqual.Check
            (
                expected: expectedUom,
                expectedNameErrors: null,
                expectedDescriptionErrors: null,
                actual: viewModel
            ));
        }

        [Test]
        [Category("UomItemViewModel")]
        public async Task SaveItemAsync()
        {
            var initialUom = new Uom(1001, "in", "inch");
            var savedUom = new Uom(1001, "in.", "inchy");
            UomItemViewModel viewModel = new ViewModelTestBuilder<Uom>()
                .Then_ReadItemService_ReadItemAsync(initialUom)
                .Then_SaveItemService_SaveItemAsync(savedUom, UomEqual.Check);
            await viewModel.GetItemAsync(1001);
            viewModel.Name = "in.";
            viewModel.Description = "inchy";
            await viewModel.SaveAsync();
        }

        [Test]
        [Category("UomItemViewModel")]
        public async Task SaveItemAsync_SaveServiceThrows_BadRequest_NameError()
        {
            var initialUom = new Uom(1001, "in", "inch");
            var savedUom = new Uom(1001, "", "inchy");
            var errorDictionary = new Dictionary<string, ReadOnlyCollection<string>>
            {
                ["Name"] = new ReadOnlyCollection<string>(new List<string> { "Error1", "Error2" })
            };
            var exception = new BadRequestHttpException(errorDictionary);
            UomItemViewModel viewModel = new ViewModelTestBuilder<Uom>()
                .Then_ReadItemService_ReadItemAsync(initialUom)
                .Then_SaveItemService_SaveItemAsync_Throws(savedUom, UomEqual.Check, exception);
            await viewModel.GetItemAsync(1001);
            viewModel.Name = "";
            viewModel.Description = "inchy";
            Assert.ThrowsAsync<BadRequestHttpException>
            (
                async () => await viewModel.SaveAsync()
            );
            Assert.True(UomItemViewModelEqual.Check
            (
                expected: savedUom,
                expectedNameErrors: "Error1 Error2",
                expectedDescriptionErrors: null,
                actual: viewModel
            ));
        }

        [Test]
        [Category("UomItemViewModel")]
        public async Task SaveItemAsync_SaveServiceThrows_BadRequest_DescriptionError()
        {
            var initialUom = new Uom(1001, "in", "inch");
            var savedUom = new Uom(1001, "in.", "");
            var errorDictionary = new Dictionary<string, ReadOnlyCollection<string>>
            {
                ["Description"] = new ReadOnlyCollection<string>(new List<string> { "Error1", "Error2" })
            };
            var exception = new BadRequestHttpException(errorDictionary);
            UomItemViewModel viewModel = new ViewModelTestBuilder<Uom>()
                .Then_ReadItemService_ReadItemAsync(initialUom)
                .Then_SaveItemService_SaveItemAsync_Throws(savedUom, UomEqual.Check, exception);
            await viewModel.GetItemAsync(1001);
            viewModel.Name = "in.";
            viewModel.Description = "";
            Assert.ThrowsAsync<BadRequestHttpException>
            (
                async () => await viewModel.SaveAsync()
            );
            Assert.True(UomItemViewModelEqual.Check
            (
                expected: savedUom,
                expectedNameErrors: null,
                expectedDescriptionErrors: "Error1 Error2",
                actual: viewModel
            ));
        }

        [Test]
        [Category("UomItemViewModel")]
        public async Task SaveItemAsync_SaveServiceThrows_BadRequest_ErrorDictionaryEmpty()
        {
            var initialUom = new Uom(1001, "in", "inch");
            var savedUom = new Uom(1001, "in.", "");
            var errorDictionary = new Dictionary<string, ReadOnlyCollection<string>>();
            var exception = new BadRequestHttpException(errorDictionary);
            UomItemViewModel viewModel = new ViewModelTestBuilder<Uom>()
                .Then_ReadItemService_ReadItemAsync(initialUom)
                .Then_SaveItemService_SaveItemAsync_Throws(savedUom, UomEqual.Check, exception);
            await viewModel.GetItemAsync(1001);
            viewModel.Name = "in.";
            viewModel.Description = "";
            var actualException = Assert.ThrowsAsync<Exception>
            (
                async () => await viewModel.SaveAsync()
            );
            Assert.AreEqual(LrpConstants.NotSavedNotDeserialized, actualException.Message);
        }

        [Test]
        [Category("UomItemViewModel")]
        public async Task SaveItemAsync_SaveServiceThrows_BadRequest_UnexpectedErrors()
        {
            var initialUom = new Uom(1001, "in", "inch");
            var savedUom = new Uom(1001, "in.", "");
            var errorDictionary = new Dictionary<string, ReadOnlyCollection<string>>
            {
                ["Unknown"] = new ReadOnlyCollection<string>(new List<string> { "Error1", "Error2" })
            };
            var exception = new BadRequestHttpException(errorDictionary);
            UomItemViewModel viewModel = new ViewModelTestBuilder<Uom>()
                .Then_ReadItemService_ReadItemAsync(initialUom)
                .Then_SaveItemService_SaveItemAsync_Throws(savedUom, UomEqual.Check, exception);
            await viewModel.GetItemAsync(1001);
            viewModel.Name = "in.";
            viewModel.Description = "";
            var actualException = Assert.ThrowsAsync<Exception>
            (
                async () => await viewModel.SaveAsync()
            );
            Assert.AreEqual(LrpConstants.NotSavedUnexpectedErrors, actualException.Message);
        }

        [Test]
        [Category("UomItemViewModel")]
        public async Task SaveItemAsync_SaveServiceThrows_EmptyBadRequest()
        {
            var initialUom = new Uom(1001, "in", "inch");
            var savedUom = new Uom(1001, "in.", "");
            var errorDictionary = new Dictionary<string, ReadOnlyCollection<string>>
            {
                ["Description"] = new ReadOnlyCollection<string>(new List<string> { "" })
            };
            var exception = new BadRequestHttpException(errorDictionary);
            UomItemViewModel viewModel = new ViewModelTestBuilder<Uom>()
                .Then_ReadItemService_ReadItemAsync(initialUom)
                .Then_SaveItemService_SaveItemAsync_Throws(savedUom, UomEqual.Check, exception);
            await viewModel.GetItemAsync(1001);
            viewModel.Name = "in.";
            viewModel.Description = "";
            var actualException = Assert.ThrowsAsync<Exception>
            (
                async () => await viewModel.SaveAsync()
            );
            Assert.AreEqual(LrpConstants.NotSavedValidationProblem, actualException.Message);
        }

        [Test]
        [Category("UomItemViewModel")]
        public async Task SaveItemAsync_SaveServiceThrows_NotBadRequest()
        {
            var initialUom = new Uom(1001, "in", "inch");
            var savedUom = new Uom(1001, "in.", "");
            var exception = new HttpRequestException();
            UomItemViewModel viewModel = new ViewModelTestBuilder<Uom>()
                .Then_ReadItemService_ReadItemAsync(initialUom)
                .Then_SaveItemService_SaveItemAsync_Throws(savedUom, UomEqual.Check, exception);
            await viewModel.GetItemAsync(1001);
            viewModel.Name = "in.";
            viewModel.Description = "";
            Assert.ThrowsAsync<HttpRequestException>
            (
                async () => await viewModel.SaveAsync()
            );
        }
    }
}
