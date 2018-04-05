using Cogi.iOS.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("Cogi")]
[assembly: ExportEffect(typeof(DoneButtonEffect), nameof(DoneButtonEffect))]
namespace Cogi.iOS.Effects
{
    [Preserve(AllMembers = true)]
    public class DoneButtonEffect: PlatformEffect
    {
        protected override void OnAttached()
        {
            var searchBar = ((UISearchBar)Control);
            searchBar.EnablesReturnKeyAutomatically = false;
        }

        protected override void OnDetached()
        {
            //throw new System.NotImplementedException();
        }
    }
}