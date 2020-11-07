namespace DomainModel.Checkout.Terminal
{
    public interface ICommandReader
    {
        string ReadCommandCode();
        decimal ReadPriceLimit();
        string ReadCancelBarCode();
    }
}