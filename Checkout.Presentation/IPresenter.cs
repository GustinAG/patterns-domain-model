namespace Checkout.Presentation
{
    public interface IPresenter
    {
        void WarnLimitExceeded(decimal limit, decimal currentPrice);
    }
}
