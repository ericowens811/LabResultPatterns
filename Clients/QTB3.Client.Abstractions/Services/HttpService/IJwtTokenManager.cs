
using System.Threading.Tasks;

namespace QTB3.Client.Abstractions.Services.HttpService
{
    public interface IJwtTokenManager : IJwtTokenSource
    {
        void UpdateRedirectUri(string uri); // used by UWP apps
        Task<bool> RequestLoginAsync();
        Task<bool> ResetPasswordAsync();
        
    }
}
