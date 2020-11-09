using System;
using Checkout.Domain.Checkout;
using Checkout.Domain.Products;

namespace Checkout.Contracts
{
    public interface ICheckoutService
    {
        void Start(Action<decimal, decimal> limitExceededAction = null);
        void Scan(string code);
        void Cancel(string code);
        void Close();
        void SetUpLimit(decimal limit);
        Bill GetCurrentBill();
        Product GetLastAdded();
        bool CanStart { get; }
        bool CanClose { get; }
    }
}
