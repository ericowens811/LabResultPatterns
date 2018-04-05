using NUnit.Framework;
using QTB3.Client.LabResultPatterns.Common.LoginComponents;
using QTB3.Test.LabResultPatterns.ClientModel.LoginComponents;

namespace QTB3.Test.LabResultPatterns.Support.Checkers.LoginComponents
{
    public class LoginPageChecker
    {
        public void Check
        (
            LoginPageModel expectedPage,
            LoginPage actualPage
        )
        {
            Assert.AreEqual(expectedPage.TitleText, actualPage.Title, "LoginPage title bad");
        }
    }
}
