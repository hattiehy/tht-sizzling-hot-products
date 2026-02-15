using ThtSizzlingHotProduct.Domain.Enums;

namespace ThtSizzlingHotProduct.Domain.Entities;

public class Order
{
    public string OrderId { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public List<OrderEntry>? Entries { get; set; }
    public DateTime Date { get; set; }
    public OrderStatus Status { get; set; }
}