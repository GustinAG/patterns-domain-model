namespace Checkout.Presentation.Commands
{
   public interface ICommand
    {
        bool CanExecute { get; }
        void Execute();
    }
}
