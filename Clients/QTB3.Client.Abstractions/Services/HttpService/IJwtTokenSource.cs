
using System.Threading.Tasks;

namespace QTB3.Client.Abstractions.Services.HttpService
{
    public interface IJwtTokenSource
    {
        Task<string> GetTokenAsync();
    }
}
