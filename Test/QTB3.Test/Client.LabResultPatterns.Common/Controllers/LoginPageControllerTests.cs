
using Moq;
using NUnit.Framework;
using QTB3.Client.Abstractions.Services;
using QTB3.Client.LabResultPatterns.Abstractions.LoginComponents;
using QTB3.Client.LabResultPatterns.Abstractions.MainPageComponents;
using QTB3.Client.LabResultPatterns.Abstractions.Navigation;
using QTB3.Client.LabResultPatterns.Common.Controllers;
using QTB3.Client.LabResultPatterns.Common.LoginComponents;
using QTB3.Client.LabResultPatterns.Common.MainPageComponents;
using QTB3.Test.Support.ConstructorTesting;
using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using QTB3.Client.Abstractions.Services.HttpService;
using QTB3.Client.LabResultPatterns.Common.Constants;
using QTB3.Client.LabResultPatterns.Common.LoginComponents.Exceptions;
using Xamarin.Forms.Mocks;

namespace QTB3.Test.Client.LabResultPatterns.Common.Controllers
{
    public class LoginPageControllerMocks
    {
        public Mock<IJwtTokenManager> JwtTokenManager { get; set; }
        public Mock<Func<ILoginPage>> CreateLoginPage { get; set; }
        public Mock<ILrpNavigation> LrpNavigation { get; set; }

        public LoginPageControllerMocks()
        {
            JwtTokenManager = new Mock<IJwtTokenManager>(MockBehavior.Strict);
            CreateLoginPage = new Mock<Func<ILoginPage>>(MockBehavior.Strict);
            LrpNavigation = new Mock<ILrpNavigation>(MockBehavior.Strict);
        }
    }

    [TestFixture]
    public class LoginPageControllerTests
    {
        [Test]
        [Category("LoginPageController")]
        public void Constructor()
        {
            var mainPage = new Mock<IMainPage>().Object;
            var jwtTokenManager = new Mock<IJwtTokenManager>().Object;
            var createLoginPage = new Mock<Func<ILoginPage>>().Object;
            var lrpNavigation = new Mock<ILrpNavigation>().Object;

            ConstructorTests<LoginPageController>
                .For
                (
                    typeof(IMainPage),
                    typeof(IJwtTokenManager),
                    typeof(Func<ILoginPage>),
                    typeof(ILrpNavigation)
                )
                .Fail(new object[] { mainPage, jwtTokenManager, createLoginPage, null }, typeof(ArgumentNullException), "Null lrpNavigation.")
                .Fail(new object[] { mainPage, jwtTokenManager, null, lrpNavigation }, typeof(ArgumentNullException), "Null createLoginPage.")
                .Fail(new object[] { mainPage, null, createLoginPage, lrpNavigation }, typeof(ArgumentNullException), "Null jwtTokenManager.")
                .Fail(new object[] { null, jwtTokenManager, createLoginPage, lrpNavigation }, typeof(ArgumentNullException), "Null mainPage.")
                .Succeed(new object[] { mainPage, jwtTokenManager, createLoginPage, lrpNavigation }, "Constructor args valid")
                .Assert();
        }

        public LoginPageController BuildController(LoginPageControllerMocks mocks, IMainPage mainPage)
        {
            return new LoginPageController
            (
                mainPage,
                mocks.JwtTokenManager.Object,
                mocks.CreateLoginPage.Object,
                mocks.LrpNavigation.Object
            );
        }

        [Test]
        [Category("LoginPageController")]
        public async Task MainPage_OnAppearingAsync()
        {
            var mocks = new LoginPageControllerMocks();
            MockForms.Init();
            var mainPage = new MainPage();
            var loginPage = new LoginPage();
            var sequence = new MockSequence();
            mocks.CreateLoginPage.InSequence(sequence).Setup(c => c()).Returns(loginPage);
            mocks.LrpNavigation.InSequence(sequence).Setup(n => n.PushModalAsync(loginPage)).Returns(Task.CompletedTask);
            BuildController(mocks, mainPage);
            await mainPage.OnAppearingAsync();
        }

        [Test]
        [Category("LoginPageController")]
        public async Task LoginPage_OnLoginClickedAsync()
        {
            var mocks = new LoginPageControllerMocks();
            MockForms.Init();
            var mainPage = new MainPage();
            var loginPage = new LoginPage();
            var sequence = new MockSequence();
            mocks.CreateLoginPage.InSequence(sequence).Setup(c => c()).Returns(loginPage);
            mocks.LrpNavigation.InSequence(sequence).Setup(n => n.PushModalAsync(loginPage)).Returns(Task.CompletedTask);
            mocks.JwtTokenManager.InSequence(sequence).Setup(j => j.RequestLoginAsync()).ReturnsAsync(true);
            mocks.LrpNavigation.InSequence(sequence).Setup(n => n.PopModalAsync()).ReturnsAsync(loginPage);
            BuildController(mocks, mainPage);
            await mainPage.OnAppearingAsync();
            await loginPage.OnLoginClickedAsync();
        }

        // for this test to work, the main page controller will need to
        // be in the mix, as it will complete the TCS after making
        // the call to the link service...
        //[Test]
        [Category("LoginPageController")]
        public async Task LoginPage_OnAppearingAsync_AfterLogin()
        {
            var mocks = new LoginPageControllerMocks();
            MockForms.Init();
            var mainPage = new MainPage();
            var loginPage = new LoginPage();
            var sequence = new MockSequence();
            mocks.CreateLoginPage.InSequence(sequence).Setup(c => c()).Returns(loginPage);
            mocks.LrpNavigation.InSequence(sequence).Setup(n => n.PushModalAsync(loginPage)).Returns(Task.CompletedTask);
            mocks.JwtTokenManager.InSequence(sequence).Setup(j => j.RequestLoginAsync()).ReturnsAsync(true);
            mocks.LrpNavigation.InSequence(sequence).Setup(n => n.PopModalAsync()).ReturnsAsync(loginPage);
            BuildController(mocks, mainPage);
            await mainPage.OnAppearingAsync();
            await loginPage.OnLoginClickedAsync();
            await mainPage.OnAppearingAsync();
        }

        // until HandleUnknownFailure implemented; 
        // review and add tests for all calls to HandleUnknownFailure
        //[Test] 
        [Category("LoginPageController")]
        public async Task LoginPage_OnLoginClickedAsync_LoginResult_False_Throws()
        {
            var mocks = new LoginPageControllerMocks();
            MockForms.Init();
            var mainPage = new MainPage();
            var loginPage = new LoginPage();
            var sequence = new MockSequence();
            mocks.CreateLoginPage.InSequence(sequence).Setup(c => c()).Returns(loginPage);
            mocks.LrpNavigation.InSequence(sequence).Setup(n => n.PushModalAsync(loginPage)).Returns(Task.CompletedTask);
            mocks.JwtTokenManager.InSequence(sequence).Setup(j => j.RequestLoginAsync()).ReturnsAsync(false);
            BuildController(mocks, mainPage);
            await mainPage.OnAppearingAsync();
            await loginPage.OnLoginClickedAsync();
            // would expect to see a modal warning popped here and then return to login page
        }

        [Test]
        [Category("LoginPageController")]
        public async Task LoginPage_OnLoginClickedAsync_Cancelled()
        {
            var mocks = new LoginPageControllerMocks();
            MockForms.Init();
            var mainPage = new MainPage();
            var loginPage = new LoginPage();
            var sequence = new MockSequence();
            mocks.CreateLoginPage.InSequence(sequence).Setup(c => c()).Returns(loginPage);
            mocks.LrpNavigation.InSequence(sequence).Setup(n => n.PushModalAsync(loginPage)).Returns(Task.CompletedTask);
            var cancelledException = new MsalException(LrpConstants.AuthenticationCanceledErrorCode);
            mocks.JwtTokenManager.InSequence(sequence).Setup(j => j.RequestLoginAsync()).Throws(cancelledException);
            BuildController(mocks, mainPage);
            await mainPage.OnAppearingAsync();
            await loginPage.OnLoginClickedAsync();
        }

        [Test]
        [Category("LoginPageController")]
        public async Task LoginPage_OnLoginClickedAsync_PasswordReset()
        {
            var mocks = new LoginPageControllerMocks();
            MockForms.Init();
            var mainPage = new MainPage();
            var loginPage = new LoginPage();
            var sequence = new MockSequence();
            mocks.CreateLoginPage.InSequence(sequence).Setup(c => c()).Returns(loginPage);
            mocks.LrpNavigation.InSequence(sequence).Setup(n => n.PushModalAsync(loginPage)).Returns(Task.CompletedTask);
            var passwordResetException = new Exception($"{LrpConstants.PasswordResetMessageContents}");
            mocks.JwtTokenManager.InSequence(sequence).Setup(j => j.RequestLoginAsync()).Throws(passwordResetException);
            mocks.JwtTokenManager.InSequence(sequence).Setup(j => j.ResetPasswordAsync()).ReturnsAsync(true);
            mocks.JwtTokenManager.InSequence(sequence).Setup(j => j.RequestLoginAsync()).ReturnsAsync(true);
            mocks.LrpNavigation.InSequence(sequence).Setup(n => n.PopModalAsync()).ReturnsAsync(loginPage);
            BuildController(mocks, mainPage);
            await mainPage.OnAppearingAsync();
            await loginPage.OnLoginClickedAsync();
        }
    }
}
