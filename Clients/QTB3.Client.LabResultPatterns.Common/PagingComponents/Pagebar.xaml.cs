using System.Diagnostics.CodeAnalysis;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QTB3.Client.LabResultPatterns.Common.PagingComponents
{
    [ExcludeFromCodeCoverage] // this class crashes R# code coverage
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Pagebar : ContentView
	{
	    public TapGestureRecognizer TapPageBack;
	    public TapGestureRecognizer TapPageForward;

	    public Label PagingLabel => PLabel;
	    public Image PageBack => PBack;
	    public Image PageBackDisabled => PBackDisabled;
	    public Image PageForward => PForward;
	    public Image PageForwardDisabled => PForwardDisabled;

        public Pagebar ()
		{
			InitializeComponent ();
		    TapPageBack = new TapGestureRecognizer();
		    TapPageForward = new TapGestureRecognizer();
		    PBack.GestureRecognizers.Add(TapPageBack);
            PForward.GestureRecognizers.Add(TapPageForward);
        }
	}
}