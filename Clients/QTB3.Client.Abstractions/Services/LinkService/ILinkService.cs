using System.Threading.Tasks;

namespace QTB3.Client.Abstractions.Services.LinkService
{
    public interface ILinkService
    {
        Task<string> GetLinksAsync();
    }
}
