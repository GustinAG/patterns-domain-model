using System.Collections.Generic;

namespace DomainModel.Domain.Products
{
    public class BarCode : ValueObject
    {
        public BarCode(string code)
        {
            Code = code;
        }

        public string Code { get; }

        protected override IList<object> EqualityComponents => new List<object> { Code };
    }
}
