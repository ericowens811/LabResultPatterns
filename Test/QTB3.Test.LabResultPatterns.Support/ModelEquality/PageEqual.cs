using System;
using System.Collections.Generic;
using QTB3.Model.LabResultPatterns.Paging;

namespace QTB3.Test.LabResultPatterns.Support.ModelEquality
{
    public static class PageEqual
    {
        public static bool Check<TItem>
        (
            Page<TItem> expected, 
            Page<TItem> actual, 
            Func<TItem, TItem, bool> equal
        )
        {
            var areEqual =
                expected.TotalCount == actual.TotalCount &&
                expected.Skip == actual.Skip &&
                expected.Take == actual.Take &&
                expected.Items.Count == actual.Items.Count;
            if(areEqual)
            {
                for(var i=0; i<expected.Items.Count; i++)
                {
                    if (!equal(expected.Items[i], actual.Items[i]))
                    {
                        areEqual = false;
                        break;
                    }
                }
            }
            return areEqual;
        }

        public static bool CheckItemsOnly<TItem>
        (
            Page<TItem> expected,
            List<TItem> actualItems,
            Func<TItem, TItem, bool> equal
        )
        {
            var areEqual = true;
            for (var i = 0; i < expected.Items.Count; i++)
            {
                if (!equal(expected.Items[i], actualItems[i]))
                {
                    areEqual = false;
                    break;
                }
            }

            return areEqual;
        }
    }
}
