﻿using System;
using DomainModel.Contracts;

namespace DomainModel.Checkout.Terminal
{
    internal class MainProcess
    {
        private readonly ICheckoutService _service;
        private readonly BillPresenter _presenter;
        private readonly ICommandReader _commandReader;
        private readonly CommandProcessor _processor;

        public MainProcess(ICheckoutService service, BillPresenter presenter, ICommandReader commandReader, CommandProcessor processor)
        {
            _service = service;
            _presenter = presenter;
            _commandReader = commandReader;
            _processor = processor;
        }

        internal void Run()
        {
            _service.Start(RenderLimitExceededText);

            string code;

            do
            {
                code = _commandReader.ReadCommandCode();
                _processor.Process(code);
            } while (code != CommandCode.Exit);

            _service.Close();
            _presenter.ShowClosedBill();

        }

        private static void RenderLimitExceededText(decimal limit, decimal currentPrice)
        {
            Console.WriteLine();
            Console.WriteLine($"Warning: Your limit has been exceeded (limit: € {limit}, current price: € {currentPrice})");
            Console.WriteLine();
        }
    }
}
