using System;
using System.Threading.Tasks;
using QTB3.Client.LabResultPatterns.Abstractions.MainPageComponents;

namespace QTB3.Client.LabResultPatterns.Common.MainPageComponents
{
    public class MainPageOnAppearingEventArgs : IMainPageOnAppearingEventArgs
    {
        public bool Handled { get; set; }

        public TaskCompletionSource<object> Tcs { get; }

        public MainPageOnAppearingEventArgs(TaskCompletionSource<object> tcs)
        {
            Tcs = tcs ?? throw new ArgumentNullException(nameof(tcs));
        }
    }
}
