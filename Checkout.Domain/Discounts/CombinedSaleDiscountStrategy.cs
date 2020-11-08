using System;
using System.Collections.Generic;
using Checkout.Domain.Checkout;
using Checkout.Domain.Products;
using Dawn;

namespace Checkout.Domain.Discounts
{
    internal class CombinedSaleDiscountStrategy : IDiscountStrategy
    {
        private const string FirstProductName = "Butter";
        private const string SecondProductName = "Milk";
        private const string DiscountName = FirstProductName + "-" + SecondProductName + " combined discount";
        private const decimal CombinedDiscount = 0.12M;

        private readonly IProductRepository _repository;

        internal CombinedSaleDiscountStrategy(IProductRepository repository)
        {
            _repository = repository;
        }

        public IReadOnlyList<AppliedDiscount> Calculate(BoughtProducts boughtProducts)
        {
            int firstProductCount = GetCountOf(boughtProducts, FirstProductName);
            int secondProductCount = GetCountOf(boughtProducts, SecondProductName);
            int discountAmount = Math.Min(firstProductCount, secondProductCount);
            var appliedDiscount = new AppliedDiscount(DiscountName, -discountAmount * CombinedDiscount);

            return new List<AppliedDiscount>() { appliedDiscount };
        }

        private int GetCountOf(BoughtProducts boughtProducts, string productName)
        {
            var product = _repository.FindBy(productName);
            Guard.Operation(product != null, "Repository error: null reference received");
            return boughtProducts.CountOf(product);
        }
    }
}
