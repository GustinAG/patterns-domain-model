namespace Checkout.Terminal
{
    internal class MainProcess
    {
        private readonly ICommandReader _commandReader;
        private readonly CommandProcessor _processor;

        public MainProcess(ICommandReader commandReader, CommandProcessor processor)
        {
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
        }
    }
}
