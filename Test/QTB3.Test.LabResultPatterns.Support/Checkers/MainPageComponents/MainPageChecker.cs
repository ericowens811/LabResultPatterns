using NUnit.Framework;
using QTB3.Client.LabResultPatterns.Common.MainPageComponents;
using QTB3.Test.LabResultPatterns.ClientModel.MainPageComponents;

namespace QTB3.Test.LabResultPatterns.Support.Checkers.MainPageComponents
{
    public class MainPageChecker
    {
        public void Check
        (
            MainPageModel expectedPage,
            MainPage actualPage
        )
        {
            Assert.AreEqual(expectedPage.TitleText, actualPage.Title, "MainPage title bad");
        }
    }
}
