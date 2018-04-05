using QTB3.Model.Abstractions;
using QTB3.Test.LabResultPatterns.ClientModel.CollectionPage;

namespace QTB3.Test.LabResultPatterns.Support.Checkers.ViewCellChecking
{
    public interface IViewCellChecker<TItem, TCell>
    where TItem: class, IEntity
    {
        void Check(CollectionPageModel<TItem> expectedPage, TItem expectedData, TCell actualViewCell);
    }
}
