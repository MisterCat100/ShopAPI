namespace ShopAPI.Model.Entities;

public class Client
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public List<Order> Orders { get; set; } = [];
}
