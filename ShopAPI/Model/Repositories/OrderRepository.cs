using ShopAPI.Model.EFCore;
using ShopAPI.Model.Entities;
using ShopAPI.Model.Identity;
using ShopAPI.Model.Interfaces;

namespace ShopAPI.Model.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly Context _context;
    private readonly AuthService _authService;

    public OrderRepository(
        Context context,
        AuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task CreateOrderAsync(Order order)
    {
        if (!await _authService.IsUserAuthenticated())
            return;

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }
}
