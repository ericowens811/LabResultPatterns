using QTB3.Client.LabResultPatterns.Common.Constants;
using QTB3.Client.LabResultPatterns.Common.UomComponents;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;

namespace QTB3.Test.LabResultPatterns.Support.ModelEquality
{
    public static class UomItemViewModelEqual
    {
        public static bool Check
        (
            Uom expected,
            string expectedNameErrors,
            string expectedDescriptionErrors,
            UomItemViewModel actual
        )
        {
            var expectedTitle = expected.Id != 0 ?
                $"{LrpConstants.TitleEdit}{LrpConstants.UomTitleRoot}" :
                $"{LrpConstants.TitleAdd}{LrpConstants.UomTitleRoot}";
            var expectedHasNameErrors = !string.IsNullOrWhiteSpace(expectedNameErrors);
            var expectedHasDescriptionErrors = !string.IsNullOrWhiteSpace(expectedDescriptionErrors);
            return
                expected.Name == actual.Name &&
                expectedNameErrors == actual.NameErrors &&
                expectedHasNameErrors == actual.HasNameErrors &&
                expected.Description == actual.Description &&
                expectedDescriptionErrors == actual.DescriptionErrors &&
                expectedHasDescriptionErrors == actual.HasDescriptionErrors &&
                expectedTitle == actual.Title;
        }
    }
}
