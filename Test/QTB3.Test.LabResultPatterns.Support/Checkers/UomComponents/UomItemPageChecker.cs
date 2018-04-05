using System.Threading.Tasks;
using NUnit.Framework;
using QTB3.Client.LabResultPatterns.Common.UomComponents;
using QTB3.Test.LabResultPatterns.ClientModel.UomComponents;

namespace QTB3.Test.LabResultPatterns.Support.Checkers.UomComponents
{
    public class UomItemPageChecker
    {
        public async Task Check
        (
            UomItemPageModel expectedPage,
            UomItemPage actualPage
        )
        {
            Assert.AreEqual(expectedPage.TitleText, actualPage.Title, "UomItemPage Title bad");
            Assert.AreEqual(expectedPage.Name, actualPage.NameEntry.Text, "UomItemPage Name bad");
            Assert.AreEqual(expectedPage.Description, actualPage.DescriptionEntry.Text, "UomItemPage Description bad");
        }
    }
}
