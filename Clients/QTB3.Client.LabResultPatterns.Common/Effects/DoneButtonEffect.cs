using QTB3.Client.LabResultPatterns.Common.Constants;
using Xamarin.Forms;

namespace QTB3.Client.LabResultPatterns.Common.Effects
{
    public class DoneButtonEffect:RoutingEffect
    {
        // used for iOS searchbar to allow searching with empty search string
        public DoneButtonEffect() : base(LrpConstants.DoneButtonEffect)
        {
        }
    }
}
