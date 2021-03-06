﻿using System;
using Checkout.Domain.Checkout;

namespace Checkout.Contracts
{
    public interface ICheckoutService
    {
        void Start();
        void Scan(string code);
        void Cancel(string code);
        void Close();
        void SetUpLimit(decimal limit);
        Bill GetCurrentBill();
        bool CanStart { get; }
        bool CanScan { get; }
        bool CanClose { get; }
        bool CanCancel { get; }
        bool CanSetUpLimit { get; }
        void SetCustomerBirthDate(DateTime birthDate);
    }
}
