
using System;
using System.Threading.Tasks;

namespace QTB3.Client.Abstractions.Views
{
    public interface IItemPage<TItem>
    {
        object BindingContext { get; set; }

        event Action<TaskCompletionSource<object>> OnAppearingCalledEvent;
        event Action<TaskCompletionSource<object>> BackButtonPressedEvent;
        event Action<TaskCompletionSource<object>> SaveClickedEvent;

        void InitializePage();
    }
}
