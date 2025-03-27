using ShopAPI.Model.Entities;

namespace ShopAPI.Model.Interfaces;

public interface IOrderRepository
{
    public Task CreateOrderAsync(Order order);
}
