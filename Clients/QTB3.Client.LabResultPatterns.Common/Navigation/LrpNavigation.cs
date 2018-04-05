
using System.Collections.Generic;
using System.Threading.Tasks;
using QTB3.Client.LabResultPatterns.Abstractions.Navigation;
using Xamarin.Forms;

namespace QTB3.Client.LabResultPatterns.Common.Navigation
{
    public class LrpNavigation : ILrpNavigation
    {
        private readonly INavigation _navigation;

        public LrpNavigation(INavigation navigation)
        {
            _navigation = navigation;
        }

        public IReadOnlyList<Page> NavigationStack => _navigation.NavigationStack;
        public IReadOnlyList<Page> ModalStack => _navigation.ModalStack;
        
        public async Task PushAsync(Page page)
        {
            await _navigation.PushAsync(page);
        }

        public async Task PushModalAsync(Page page)
        {
            await _navigation.PushModalAsync(page);
        }

        public async Task<Page> PopAsync()
        {
            return await _navigation.PopAsync();
        }

        public async Task<Page> PopModalAsync()
        {
            return await _navigation.PopModalAsync();
        }
    }
}
