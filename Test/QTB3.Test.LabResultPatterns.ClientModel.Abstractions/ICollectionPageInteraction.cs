using System.Threading.Tasks;

namespace QTB3.Test.LabResultPatterns.ClientModel.Abstractions
{
    public interface ICollectionPageInteraction<TItem>
    {
        string TitleText { get; set; }
        void ClickEditOnToolbar();
        void ClickDeleteOnToolbar();
        void SetSearchText(string text);
        Task Search();
        TItem Select(int index);
        Task DeleteAsync(int index);
        Task PageBackTappedAsync();
        Task PageForwardTappedAsync();
        Task OnAppearingAsync();
    }
}
