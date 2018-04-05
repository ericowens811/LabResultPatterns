
using System.Collections.Immutable;

namespace QTB3.Model.Abstractions
{
    public interface IPage<TItem>
    {
        string SearchText { get; }
        int TotalCount { get; }
        int Skip { get; }
        int Take { get; }
        IImmutableList<TItem> Items { get; }
    }
}
