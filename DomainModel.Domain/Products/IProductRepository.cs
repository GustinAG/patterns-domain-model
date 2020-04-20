namespace DomainModel.Domain.Products
{
    public interface IProductRepository
    {
        Product FindBy(BarCode barCode);
        Product FindBy(string name);
    }
}
