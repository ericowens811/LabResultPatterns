using System;
using QTB3.Client.Common.Services;
using QTB3.Client.Common.Services.Data;
using QTB3.Client.LabResultPatterns.Common.ViewModels;
using QTB3.Model.Abstractions;

namespace QTB3.Test.LabResultPatterns.Support.Checkers.ViewModelChecking
{
    public class CollectionPageViewModelChecker
    {
        public static bool Check<T>
        (
            CollectionPageData<T> expectedData,
            CollectionPageViewModel<T> actualViewModel,
            Func<T, T, bool> equal
        )
        where T: class, IEntity
        {
            var areEqual =
                false == actualViewModel.IsBusy &&
                expectedData.HasBackPages == actualViewModel.HasBackPages &&
                expectedData.HasForwardPages == actualViewModel.HasForwardPages &&
                expectedData.IsPaging == actualViewModel.IsPaging &&
                expectedData.PagingText == actualViewModel.PagingText;
            if (!areEqual) return false;
            for (var i = 0; i < expectedData.Items.Count; i++)
            {
                if (!equal(expectedData.Items[i], actualViewModel.Items[i]))
                {
                    areEqual = false;
                    break;
                }
            }

            return areEqual;
        }
    }
}
