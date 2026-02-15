using Moq;
using ThtSizzlingHotProduct.Application.Services;
using ThtSizzlingHotProduct.Domain.Entities;
using ThtSizzlingHotProduct.Domain.Enums;
using ThtSizzlingHotProduct.Domain.Interfaces;
using Xunit;

namespace ThtSizzlingHotProduct.Tests.Application.Services;

public class ProcessSalesServiceTests
{
    private readonly Mock<IOrderRepository> _orderRepoMock;
    private readonly ProcessSalesService _service;

    public ProcessSalesServiceTests()
    {
        _orderRepoMock = new Mock<IOrderRepository>();
        _service = new ProcessSalesService(_orderRepoMock.Object);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ProcessSalesService(null!));
    }

    [Fact]
    public async Task GetSalesScoreByDateAsync_ShouldCalculateCorrectScores_WhenNoCancellations()
    {
        // Arrange
        var start = new DateTime(2026, 2, 1);
        var end = new DateTime(2026, 2, 1);

        var orders = new List<Order>
        {
            new Order {
                OrderId = "O1", CustomerId = "C1", Date = start, Status = OrderStatus.Completed,
                Entries = new List<OrderEntry> { new OrderEntry { id = "P1" }, new OrderEntry { id = "P2" } }
            },
            new Order {
                OrderId = "O2", CustomerId = "C2", Date = start, Status = OrderStatus.Completed,
                Entries = new List<OrderEntry> { new OrderEntry { id = "P1" } }
            }
        };

        _orderRepoMock.Setup(r => r.GetOrdersByDateRangeWithRelated(start, end))
            .ReturnsAsync(orders);

        // Act
        var result = await _service.GetSalesScoreByDateAsync(start, end);

        // Assert
        Assert.Equal(2, result["P1"]);
        Assert.Equal(1, result["P2"]);
    }

    [Fact]
    public async Task GetSalesScoreByDateAsync_ShouldCountOnce_WhenSameCustomerBuysSameProductAtSameDay()
    {
        // Arrange
        var date1 = new DateTime(2026, 2, 19, 10, 0, 0);
        var date2 = new DateTime(2026, 2, 19, 14, 0, 0);
        var start = new DateTime(2026, 2, 19);
        var end = new DateTime(2026, 2, 19);

        var orders = new List<Order>
        {
            new Order
            {
                OrderId = "O1", CustomerId = "C1", Date = date1, Status = OrderStatus.Completed,
                Entries = new List<OrderEntry> { new OrderEntry { id = "P1" } }
            },
            new Order
            {
                OrderId = "O2", CustomerId = "C1", Date = date2, Status = OrderStatus.Completed,
                Entries = new List<OrderEntry> { new OrderEntry { id = "P1" } }
            }
        };

        _orderRepoMock.Setup(r => r.GetOrdersByDateRangeWithRelated(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(orders);

        // Act
        var result = await _service.GetSalesScoreByDateAsync(start, end);

        // Assert
        Assert.Equal(1, result["P1"]);
    }

    [Fact]
    public async Task GetSalesScoreByDateAsync_ShouldDeductScore_WhenOrderIsCancelled()
    {
        // Arrange
        var date = new DateTime(2026, 2, 1);
        var orders = new List<Order>
        {
            new Order {
                OrderId = "O10", CustomerId = "C1", Date = date, Status = OrderStatus.Completed,
                Entries = new List<OrderEntry> { new OrderEntry { id = "P1" } }
            },
            new Order {
                OrderId = "O10", CustomerId = "C1", Date = date, Status = OrderStatus.Cancelled
            }
        };

        _orderRepoMock.Setup(r => r.GetOrdersByDateRangeWithRelated(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(orders);

        // Act
        var result = await _service.GetSalesScoreByDateAsync(date, date);

        // Assert
        Assert.True(!result.ContainsKey("P1"));
    }

    [Fact]
    public async Task GetSalesScoreByDateAsync_ShouldHandleDuplicateEntries()
    {
        // Arrange
        var date = new DateTime(2026, 2, 1);
        var orders = new List<Order>
        {
            new Order {
                OrderId = "O20", CustomerId = "C1", Date = date, Status = OrderStatus.Completed,
                Entries = new List<OrderEntry> { new OrderEntry { id = "P1", Quantity = 5 }, new OrderEntry { id = "P1" , Quantity = 3} }
            }
        };

        _orderRepoMock.Setup(r => r.GetOrdersByDateRangeWithRelated(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(orders);

        // Act
        var result = await _service.GetSalesScoreByDateAsync(date, date);

        // Assert
        Assert.Equal(1, result["P1"]);
    }
}