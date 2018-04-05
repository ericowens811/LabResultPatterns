
using QTB3.Model.Abstractions;

namespace QTB3.Client.Abstractions.Linking
{
    public interface ILinks
    {
        string GetUrl(RelTypes rel);
        void SetUrl(RelTypes rel, string url);
    }
}
