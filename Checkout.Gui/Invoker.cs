using System;
using Checkout.Presentation;

namespace Checkout.Gui
{
    internal class Invoker
    {
        private readonly Action _refreshDisplayAction;

        internal Invoker(Action refreshDisplayAction)
        {
            _refreshDisplayAction = refreshDisplayAction;
        }

        internal void Invoke(ICommand command)
        {
            command.Execute();
            _refreshDisplayAction();
        }
    }
}
