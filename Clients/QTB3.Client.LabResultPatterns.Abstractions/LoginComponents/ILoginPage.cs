
using System;
using System.Threading.Tasks;

namespace QTB3.Client.LabResultPatterns.Abstractions.LoginComponents
{
    public interface ILoginPage
    {
        event Action<TaskCompletionSource<object>> LoginClickedEvent;
        Task OnLoginClickedAsync();
    }
}
