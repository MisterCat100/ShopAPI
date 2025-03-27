namespace ShopAPI.Model.Entities;

public class Product
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public List<OrderProduct> OrderProducts { get; set; } = [];
}
