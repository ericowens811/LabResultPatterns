
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;

namespace QTB3.Test.LabResultPatterns.Support.ModelEquality
{
    public static class UomEqual
    {
        public static bool Check(Uom expected, Uom actual)
        {
            return
                expected.Id == actual.Id &&
                expected.Name == actual.Name &&
                expected.Description == actual.Description;
        }

        public static bool CheckExceptId(Uom expected, Uom actual)
        {
            return
                expected.Name == actual.Name &&
                expected.Description == actual.Description;
        }
    }
}
