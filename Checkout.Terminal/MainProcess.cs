namespace Checkout.Terminal
{
    internal class MainProcess
    {
        private readonly BillPresenter _presenter;
        private readonly ICommandReader _commandReader;
        private readonly CommandProcessor _processor;

        public MainProcess(BillPresenter presenter, ICommandReader commandReader, CommandProcessor processor)
        {
            _presenter = presenter;
            _commandReader = commandReader;
            _processor = processor;
        }

        internal void Run()
        {
            _processor.Start();

            string code;

            do
            {
                code = _commandReader.ReadCommandCode();
                _processor.Process(code);
            } while (code != CommandCode.Exit);

            _processor.Stop();
            _presenter.ShowClosedBill();
        }
    }
}
