using Checkout.Contracts;

namespace Checkout.Presentation
{
    public interface IPresenter : IWarningPresenter
    {
        void RefreshDisplay(BillAppearance appearance);
        void ShowError(string message);
    }
}
