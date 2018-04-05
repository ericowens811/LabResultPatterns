using System.Threading.Tasks;

namespace QTB3.Test.LabResultPatterns.ClientModel.MainPageComponents
{
    public class MainPageModel
    {
        public MainPageModel
        (
            string titleText
        )
        {
            TitleText = titleText;
        }

        public string TitleText { get; }

        public async Task OnAppearingAsync()
        {
        }
    }
}
