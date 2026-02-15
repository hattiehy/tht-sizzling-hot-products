using ThtSizzlingHotProduct.Domain.Interfaces;
using ThtSizzlingHotProduct.Application.Interfaces;

namespace ThtSizzlingHotProduct.Application.Services;

public class ProcessSalesService : IProcessSalesService
{
    private readonly IOrderRepository _orderRepository;
    public ProcessSalesService(IOrderRepository orderedDictionary)
    {
        _orderRepository = orderedDictionary ?? throw new ArgumentNullException(nameof(orderedDictionary));
    }

    public async Task<Dictionary<string, int>> GetSalesScoreByDateAsync(DateTime start, DateTime end)
    {
        var orders = await _orderRepository.GetOrdersByDateRangeWithRelated(start, end);

        var uniqueSales = new HashSet<(DateTime OrderDate, string CustomerId, string ProductId)>();
        var completedOrderDictionary = orders.Where(o =>
        ((o.Status == Domain.Enums.OrderStatus.Completed) && o.Entries != null))
        .ToDictionary(o => o.OrderId, o => o);

        foreach (var order in completedOrderDictionary.Values)
        {
            foreach (var entry in order.Entries!)
            {
                uniqueSales.Add((order.Date.Date, order.CustomerId, entry.id));
            }
        }

        foreach (var cancelled in orders.Where(o => o.Status == Domain.Enums.OrderStatus.Cancelled))
        {
            if (completedOrderDictionary.TryGetValue(cancelled.OrderId, out var origialOrder))
            {
                foreach (var entry in origialOrder.Entries!)
                {
                    uniqueSales.Remove((origialOrder.Date.Date, origialOrder.CustomerId, entry.id));
                }
            }
        }

        var sales = uniqueSales.GroupBy(s => s.ProductId).ToList();
        return sales.ToDictionary(x => x.Key, x => x.Count());
    }
}