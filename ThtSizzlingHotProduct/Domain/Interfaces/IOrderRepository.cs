using ThtSizzlingHotProduct.Domain.Entities;

namespace ThtSizzlingHotProduct.Domain.Interfaces;

public interface IOrderRepository
{
    public Task<List<Order>> GetOrdersByDateRangeWithRelated(DateTime start, DateTime end);
}