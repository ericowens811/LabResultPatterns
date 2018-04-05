
using System;
using QTB3.Client.Abstractions.Paging;
using QTB3.Client.Common.Services;
using QTB3.Client.Common.Services.Data;
using Xamarin.Forms;

namespace QTB3.Test.LabResultPatterns.Support.ModelEquality
{
    public static class CollectionPageDataEqual
    {
        public static bool Check<T>
        (
            CollectionPageData<T> expected,
            CollectionPageData<T> actual,
            Func<T, T, bool> equal
        )
        {
            return CheckInternal(expected, actual, equal);
        }

        public static bool Check<T>
        (
            CollectionPageData<T> expected,
            ICollectionPageData<T> actual,
            Func<T, T, bool> equal
        )
        {
            return CheckInternal(expected, actual as CollectionPageData<T>, equal);
        }

        private static bool CheckInternal<T>
        (
            CollectionPageData<T> expected,
            CollectionPageData<T> actual,
            Func<T, T, bool> equal
        )
        {
            var areEqual =
                expected.HasBackPages == actual.HasBackPages &&
                expected.HasForwardPages == actual.HasForwardPages &&
                expected.IsPaging == actual.IsPaging &&
                expected.PagingText == actual.PagingText &&
                expected.Items.Count == actual.Items.Count &&
                expected.Links.Count == actual.Links.Count;

            if (!areEqual) return false;
            foreach (var link in expected.Links)
            {
                actual.Links.TryGetValue(link.Key, out var url);
                if (link.Value != url)
                {
                    areEqual = false;
                    break;
                }
            }

            if (!areEqual) return false;
            for (var i = 0; i < expected.Items.Count; i++)
            {
                if (!equal(expected.Items[i], actual.Items[i]))
                {
                    areEqual = false;
                    break;
                }
            }
            return areEqual;
        }

    }
}
