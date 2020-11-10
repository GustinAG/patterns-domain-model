namespace Checkout.Presentation
{
    public interface IPresenter
    {
        void RefreshDisplay(BillAppearance appearance);
        void ShowWarning(string message);
        void ShowError(string message);
    }
}
