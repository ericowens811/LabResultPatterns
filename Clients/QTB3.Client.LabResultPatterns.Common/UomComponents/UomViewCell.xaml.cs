using System.Diagnostics.CodeAnalysis;
using QTB3.Client.Abstractions.Views;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QTB3.Client.LabResultPatterns.Common.UomComponents
{
    [ExcludeFromCodeCoverage] // this class crashes R# code coverage
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UomViewCell : IListItem<Uom>
	{
	    public IToolbar PageView { get; set; }

	    public Label NameLabel { get; }
	    public Label DescriptionLabel { get; }
	    public Image DeleteIcon { get; }
	    public Image EditIcon { get; }

        public UomViewCell (IToolbar pageView)
        {
            PageView = pageView;
			InitializeComponent ();
            NameLabel = ItemNameLabel;
            DescriptionLabel = ItemDescriptionLabel;
            DeleteIcon = ItemDeleteIcon;
            EditIcon = ItemEditIcon;
        }
	}
}