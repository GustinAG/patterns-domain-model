namespace Checkout.Presentation
{
   public interface ICommand
    {
        bool CanExecute { get; }
        void Execute();
    }
}
