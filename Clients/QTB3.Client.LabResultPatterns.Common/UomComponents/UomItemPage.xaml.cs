using System.Diagnostics.CodeAnalysis;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QTB3.Client.LabResultPatterns.Common.UomComponents
{
    [ExcludeFromCodeCoverage] // this class crashes R# code coverage
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UomItemPage: Views.ItemPage<Uom>
	{
	    public Entry NameEntry;
	    public Entry DescriptionEntry;
        public UomItemPage()
        {
            InitializeComponent();
            NameEntry = ItemNameEntry;
            DescriptionEntry = ItemDescriptionEntry;
        }
    }
}