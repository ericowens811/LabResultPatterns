using System.Collections.Immutable;

namespace QTB3.Api.Abstractions.Utilities
{
    public interface ISupportedMedia
    {
         ImmutableList<string> Types { get; }
    }
}
