using System.Text.Json;
using System.Text.Json.Serialization;
using ThtSizzlingHotProduct.Domain.Entities;
using ThtSizzlingHotProduct.Domain.Interfaces;
using ThtSizzlingHotProduct.Infrastructure.Persistence.Converters;

namespace ThtSizzlingHotProduct.Infrastructure.Persistence.Repositories;

public class JsonOrderRepository : IOrderRepository
{
    private readonly string _inputFolderPath;

    public JsonOrderRepository(string inputFolderPath)
    {
        _inputFolderPath = inputFolderPath ?? throw new ArgumentNullException(nameof(inputFolderPath));
    }

    public async Task<List<Order>> GetOrdersByDateRangeWithRelated(DateTime start, DateTime end)
    {
        var allOrders = await LoadAllOrdersFromInputFileAsync();

        var rangeOrders = allOrders.Where(o =>
            o.Date.Date >= start.Date && o.Date.Date <= end.Date)
            .ToList();

        var rangeOrderIds = rangeOrders.Select(o => o.OrderId).ToHashSet();

        var relatedOrders = allOrders
            .Where(o => rangeOrderIds.Contains(o.OrderId) && !rangeOrders.Any(r => r.OrderId == o.OrderId && r.Status == o.Status));

        return rangeOrders.Concat(relatedOrders).ToList();
    }

    private async Task<List<Order>> LoadAllOrdersFromInputFileAsync()
    {
        var ordersFilePath = Path.Combine(_inputFolderPath, "orders.json");

        if (!File.Exists(ordersFilePath))
        {
            throw new FileNotFoundException($"Orders file not found: {ordersFilePath}");
        }

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        options.Converters.Add(new DateTimeConverter());

        var json = await File.ReadAllTextAsync(ordersFilePath);
        var orders = JsonSerializer.Deserialize<List<Order>>(json, options);

        return orders ?? new List<Order>();
    }
}