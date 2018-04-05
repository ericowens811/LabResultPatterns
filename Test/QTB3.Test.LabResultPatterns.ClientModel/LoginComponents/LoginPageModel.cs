namespace QTB3.Test.LabResultPatterns.ClientModel.LoginComponents
{
    public class LoginPageModel
    {
        public LoginPageModel
        (
            string titleText
        )
        {
            TitleText = titleText;
        }
        public string TitleText { get; }

        public void OnAppearing()
        {
        }
    }
}
