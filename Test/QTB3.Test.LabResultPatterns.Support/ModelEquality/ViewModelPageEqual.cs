using System;
using QTB3.Client.LabResultPatterns.Common.ViewModels;
using QTB3.Model.Abstractions;
using QTB3.Model.LabResultPatterns.Paging;
using QTB3.Test.LabResultPatterns.ClientModel.Pagebar;

namespace QTB3.Test.LabResultPatterns.Support.ModelEquality
{
    public static class ViewModelPageEqual
    {
        public static bool Check<TItem>
        (
            Page<TItem> expected,
            CollectionPageViewModel<TItem> actual,
            Func<TItem, TItem, bool> equal
        )
            where TItem : class, IEntity
        {
            var pagingText = PagingTextCalculator.Calculate(expected.Skip, expected.Take, expected.TotalCount);
            var hasForwardPages = expected.TotalCount > expected.Skip + expected.Take;
            var hasBackPages = expected.Skip > 0;
            var isPaging = expected.TotalCount > expected.Take;
            var result =
                expected.Items.Count == actual.Items.Count &&
                pagingText == actual.PagingText &&
                hasForwardPages == actual.HasForwardPages &&
                hasBackPages == actual.HasBackPages &&
                isPaging == actual.IsPaging;
            if (result)
            {
                for (var i = 0; i < expected.Items.Count; i++)
                {
                    if (!equal(expected.Items[i], actual.Items[i]))
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }
    }
}
