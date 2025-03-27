namespace ShopAPI.Model.Entities;

public class Order
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public required Client Client { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderProduct> OrderProducts { get; set; } = [];
}
