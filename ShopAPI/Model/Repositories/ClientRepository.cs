using Microsoft.EntityFrameworkCore;
using ShopAPI.Model.EFCore;
using ShopAPI.Model.Entities;
using ShopAPI.Model.Identity;
using ShopAPI.Model.Interfaces;

namespace ShopAPI.Model.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly Context _context;
    private readonly AuthService _authService;

    public ClientRepository(
        Context context,
        AuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task AddClientAsync(Client client)
    {
        if (!await _authService.IsUserAuthenticated())
            return;

        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Order>?> GetOrdersAsync(Guid id)
    {
        if (!await _authService.IsUserAuthenticated())
            return null;

        Client? client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);

        if (client != null)
        {
            var groupedByProduct = client.Orders
                .GroupBy(o => o.OrderProducts)
                .Select(g => new Order
                {
                    Client = client,
                    TotalPrice = g.Sum(o => o.OrderProducts.Sum(op => op.Product.Price))
                })
                .ToList();

            return groupedByProduct;
        }

        return null;
    }
}
