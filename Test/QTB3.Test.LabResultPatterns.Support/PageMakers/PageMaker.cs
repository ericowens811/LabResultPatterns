using System.Linq;
using QTB3.Model.Abstractions;
using QTB3.Model.LabResultPatterns.Paging;

namespace QTB3.Test.LabResultPatterns.Support.PageMakers
{
    public static class PageMaker
    {
        public static Page<T> GetExpectedPage<T>(T[] data, string searchText, int skip, int take)
        where T: IEntity
        {
            var expectedItems = data
                .Where(u => string.IsNullOrWhiteSpace(searchText) || u.Name.Contains(searchText))
                .OrderBy(u => u.Name)
                .Skip(skip)
                .Take(take)
                .ToList();

            var expectedTotalCount = data
                .Count(u => string.IsNullOrWhiteSpace(searchText) || u.Name.Contains(searchText));

            return new Page<T>
            (
                searchText: searchText,
                totalCount : expectedTotalCount,
                skip: skip,
                take: take,
                items : expectedItems
            );
        }
    }
}
