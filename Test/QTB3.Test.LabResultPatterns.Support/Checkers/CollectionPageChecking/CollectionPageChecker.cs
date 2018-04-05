using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using QTB3.Client.LabResultPatterns.Common.Views;
using QTB3.Model.Abstractions;
using QTB3.Test.LabResultPatterns.ClientModel.CollectionPage;
using QTB3.Test.LabResultPatterns.Support.Checkers.ViewCellChecking;

namespace QTB3.Test.LabResultPatterns.Support.Checkers.CollectionPageChecking
{
    public class CollectionPageChecker<TItem, TCell>
        where TItem : class, IEntity
        where TCell: class
    {
        protected readonly IViewCellChecker<TItem, TCell> CellChecker;
        protected Func<DbContext> GetDeviceContext;
        protected Func<DbContext> GetModelContext;
        protected Func<TItem, TItem, bool> CheckItem;

        public CollectionPageChecker
        (
            IViewCellChecker<TItem, TCell> cellChecker,
            Func<DbContext> getDeviceContext,
            Func<DbContext> getModelContext,
            Func<TItem, TItem, bool> checkItem
        )
        {
            CellChecker = cellChecker;
            GetDeviceContext = getDeviceContext;
            GetModelContext = getModelContext;
            CheckItem = checkItem;
        }

        public async Task Check
        (
            CollectionPageModel<TItem> expectedPage,
            CollectionPage<TItem> actualPage
        )
        {
            CheckTitleAndSearchText(expectedPage, actualPage);
            CheckToolbarEnables(expectedPage, actualPage);
            CheckViewCells(expectedPage, actualPage);
            CheckPaging(expectedPage, actualPage);
            await CheckDb();
        }

        protected virtual void CheckTitleAndSearchText
        (
            CollectionPageModel<TItem> expectedPage,
            CollectionPage<TItem> actualPage
        )
        {
            Assert.AreEqual(expectedPage.TitleText, actualPage.Title);
            Assert.AreEqual(expectedPage.SearchText, actualPage.SearchBar.Text);
        }

        protected void CheckToolbarEnables
        (
            CollectionPageModel<TItem> expectedPage,
            CollectionPage<TItem> actualPage
        )
        {
            Assert.AreEqual(true, actualPage.AddToolbarItem.IsEnabled);
            Assert.AreEqual(true, actualPage.EditToolbarItem.IsEnabled);
            Assert.AreEqual(true, actualPage.DeleteToolbarItem.IsEnabled);
        }

        protected void CheckViewCells
        (
            CollectionPageModel<TItem> expectedPage,
            CollectionPage<TItem> actualPage
        )
        {
            var viewCells = actualPage.ViewCellsProbe;
            Assert.NotNull(viewCells);
            Assert.AreEqual(expectedPage.ExpectedPage.Items.Count, viewCells.Count);
            var expectedItems = expectedPage.ExpectedPage.Items;
            for (var i = 0; i < expectedItems.Count; i++)
            {
                var castCell = actualPage.ViewCellsProbe[i] as TCell;
                Assert.NotNull(castCell);
                CellChecker.Check(expectedPage, expectedItems[i], castCell);
            }
        }

        protected async Task CheckDb
        (
        )
        {
            using (var deviceContext = GetDeviceContext())
            using (var modelContext = GetModelContext())
            {
                var deviceDbItems = await deviceContext.Set<TItem>()
                    .OrderBy(i => i.Name)
                    .ToListAsync();

                var modelDbItems = await modelContext.Set<TItem>()
                    .OrderBy(i => i.Name)
                    .ToListAsync();

                Assert.AreEqual(modelDbItems.Count, deviceDbItems.Count);
                for(var i=0; i<modelDbItems.Count; i++)
                {
                    Assert.True(CheckItem(modelDbItems[i], deviceDbItems[i]));
                }
            }
        }

        protected void CheckPaging
        (
            CollectionPageModel<TItem> expectedPage,
            CollectionPage<TItem> actualPage
        )
        {
            var actualPagebar = actualPage.Pagebar;
            var expectedPagebar = expectedPage.ExpectedPagebar;

            Assert.AreEqual(expectedPagebar.IsVisible, actualPagebar.IsVisible);
            if (actualPagebar.IsVisible)
            {
                Assert.AreEqual(expectedPagebar.PageText, actualPagebar.PagingLabel.Text);
                Assert.AreEqual(expectedPagebar.PageForwardIsVisible, actualPagebar.PageForward.IsVisible);
                Assert.AreEqual(expectedPagebar.PageForwardDisabledIsVisible, actualPagebar.PageForwardDisabled.IsVisible);
                Assert.AreEqual(expectedPagebar.PageBackIsVisible, actualPagebar.PageBack.IsVisible);
                Assert.AreEqual(expectedPagebar.PageBackDisabledIsVisible, actualPagebar.PageBackDisabled.IsVisible);
            }
        }
    }
}
