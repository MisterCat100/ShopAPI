using ShopAPI.Model.EFCore;
using ShopAPI.Model.Entities;
using ShopAPI.Model.Identity;
using ShopAPI.Model.Interfaces;

namespace ShopAPI.Model.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly Context _context;
    private readonly AuthService _authService;

    public ProductRepository(
        Context context,
        AuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task AddProductAsync(Product product)
    {
        if (!await _authService.IsUserAuthenticated())
            return;
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Product>?> GetProductsByDate(DateTime startDate, DateTime endDate)
    {
        if (!await _authService.IsUserAuthenticated())
            return null;

        return _context.Products
            .Where(p => p.OrderProducts.Any(op => op.Order.OrderDate >= startDate
                        && op.Order.OrderDate <= endDate))
            .ToList();
    }
}
