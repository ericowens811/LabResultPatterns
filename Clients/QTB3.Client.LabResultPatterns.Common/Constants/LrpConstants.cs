
namespace QTB3.Client.LabResultPatterns.Common.Constants
{
    public static class LrpConstants
    {
        public const int PageSize = 20;
        // Login
        public const string PasswordResetMessageContents = "AADB2C90118";
        public const string AuthenticationCanceledErrorCode = "authentication_canceled";

        // MainPage
        public const string MainPageTitle = "Home";

        // CollectionPage
        public const string SearchFor = "Search for...";

        // Uom
        public const string UomPageTitle = "Uoms";
        public const string UomTitleRoot = "Uom";
        public const string TitleEdit = "Edit ";
        public const string TitleAdd = "Add a new ";

        // ItemViewModel
        public const string NotSavedNotDeserialized = "Item not saved. Could not deserialize validation errors.";
        public const string NotSavedUnexpectedErrors = "Item not saved. There were unexpected validation errors.";
        public const string NotSavedValidationProblem = "Item not saved. There was a problem with validation";

        // Effects
        public const string DoneButtonEffect = "QTB3.LabResultPatterns.Client.Common.Effects.DoneButtonEffect";        
    }
}
