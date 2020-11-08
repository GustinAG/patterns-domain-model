﻿using System;

namespace Checkout.Contracts
{
    public interface ICheckoutService
    {
        void Start(Action<decimal, decimal> limitExceededAction = null);
        void Scan(string code);
        void Cancel(string code);
        void Close();
        void SetUpLimit(decimal limit);
        string GetCurrentBill();
        string GetLastAdded();
        bool CanStart { get; }
    }
}
