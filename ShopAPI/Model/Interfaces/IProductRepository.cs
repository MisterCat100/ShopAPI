using ShopAPI.Model.Entities;

namespace ShopAPI.Model.Interfaces;

public interface IProductRepository
{
    public Task AddProductAsync(Product product);
    public Task<List<Product>?> GetProductsByDate(DateTime startDate, DateTime endDate);
}
