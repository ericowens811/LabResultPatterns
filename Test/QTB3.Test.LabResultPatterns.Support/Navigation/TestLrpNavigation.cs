
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QTB3.Client.Abstractions.Views;
using QTB3.Client.LabResultPatterns.Abstractions.Navigation;
using Xamarin.Forms;

namespace QTB3.Test.LabResultPatterns.Support.Navigation
{
    public class TestLrpNavigation : ILrpNavigation
    {
        private readonly INavigation _navigation;

        public TestLrpNavigation(INavigation navigation)
        {
            _navigation = navigation;
        }

        public IReadOnlyList<Page> NavigationStack => _navigation.NavigationStack;
        public IReadOnlyList<Page> ModalStack => _navigation.ModalStack;

        // Xamarin.Forms.Mocks does not run the View lifecycle methods,
        // so we do it with the next two methods

        public async Task PushAsync(Page page)
        {
            await _navigation.PushAsync(page);
            await (page as IOnAppearingAsync).OnAppearingAsync();
        }

        public async Task PushModalAsync(Page page)
        {
            await _navigation.PushModalAsync(page);
            await (page as IOnAppearingAsync).OnAppearingAsync();
        }

        public async Task<Page> PopAsync()
        {
            var page = await _navigation.PopAsync();
            var revealedPage = _navigation.NavigationStack.Last();
            await (revealedPage as IOnAppearingAsync).OnAppearingAsync();
            return page;
        }

        public async Task<Page> PopModalAsync()
        {
            var page = await _navigation.PopModalAsync();
            if (_navigation.ModalStack.Count > 0)
            {
                var revealedPage = _navigation.ModalStack.Last();
                await (revealedPage as IOnAppearingAsync).OnAppearingAsync();
            }
            else
            {
                var revealedPage = _navigation.NavigationStack.Last();
                await (revealedPage as IOnAppearingAsync).OnAppearingAsync();
            }
            return page;
        }
    }
}
