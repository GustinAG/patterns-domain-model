using Dawn;
using DomainModel.Domain.Checkout;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.Domain.Limiter
{
    public class CreditLimit
    {
        public Decimal CashLimit { get; set; }

        private decimal GetCurrentBillValue(OutChecker _outChecker)
        {
            Guard.Operation(_outChecker != null);
            return _outChecker.ShowBill().SumPriceOfBoughtProducts;
        }

        public string GetStringInCaseOfCashLimit(OutChecker _outChecker)
        {
            Guard.Operation(_outChecker != null);
            var remind = CashLimit - GetCurrentBillValue(_outChecker);
            var retString = "\n";
            if (remind <= 0)
            {
                retString += "You reach the limit, that is waring, please close the bill... :) \n";
                retString += _outChecker.ShowBill().PrintableText + " \n";
            }
            else
            {
                retString += string.Format("Reminder rest of cache {0}", remind);
            }
            return retString;
        }
    }
}
