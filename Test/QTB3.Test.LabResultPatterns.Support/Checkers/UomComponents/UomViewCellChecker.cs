using NUnit.Framework;
using QTB3.Client.LabResultPatterns.Common.UomComponents;
using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.LabResultPatterns.ClientModel.CollectionPage;
using QTB3.Test.LabResultPatterns.Support.Checkers.ViewCellChecking;

namespace QTB3.Test.LabResultPatterns.Support.Checkers.UomComponents
{
    public class UomViewCellChecker : IViewCellChecker<Uom, UomViewCell>
    {
        public void Check(CollectionPageModel<Uom> expectedPage, Uom expectedData, UomViewCell actualViewCell)
        {
            Assert.AreEqual(expectedPage.IsDeleting, actualViewCell.DeleteIcon.IsVisible);
            Assert.AreEqual(expectedPage.IsEditing, actualViewCell.EditIcon.IsVisible);
            Assert.AreEqual(expectedData.Name, actualViewCell.NameLabel.Text);
            Assert.AreEqual(expectedData.Description, actualViewCell.DescriptionLabel.Text);
        }
    }
}
