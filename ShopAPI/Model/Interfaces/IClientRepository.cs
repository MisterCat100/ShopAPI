using ShopAPI.Model.Entities;

namespace ShopAPI.Model.Interfaces;

public interface IClientRepository
{
    public Task AddClientAsync(Client client);
    public Task<List<Order>?> GetOrdersAsync(Guid id);
}
