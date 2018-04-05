using System.Collections.Immutable;
using QTB3.Model.Abstractions;

namespace QTB3.Client.Abstractions.Paging
{
    public interface ICollectionPageData<TItem>
    {
        IImmutableList<TItem> Items { get; }
        bool IsPaging { get; }
        bool HasForwardPages { get; }
        bool HasBackPages { get; }
        string PagingText { get;  }
        IImmutableDictionary<RelTypes, string> Links { get; }
    }
}
