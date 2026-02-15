using Moq;
using Xunit;
using ThtSizzlingHotProduct.Application.Interfaces;
using ThtSizzlingHotProduct.Application.Services;
using ThtSizzlingHotProduct.Domain.Interfaces;
using ThtSizzlingHotProduct.Domain.Entities;

namespace ThtSizzlingHotProduct.Tests.Application.Services;

public class SizzlingProductServiceTests
{
    private readonly Mock<IProcessSalesService> _salesServiceMock;
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly SizzlingProductService _service;

    public SizzlingProductServiceTests()
    {
        _salesServiceMock = new Mock<IProcessSalesService>();
        _productRepoMock = new Mock<IProductRepository>();
        _service = new SizzlingProductService(_salesServiceMock.Object, _productRepoMock.Object);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new SizzlingProductService(null!, null!));
    }

    [Fact]
    public async Task GetTopProductByDayAsync_ShouldReturnDefaultMessage_WhenNoSalesFound()
    {
        // Arrange
        var testDate = new DateTime(2026, 2, 16);
        _salesServiceMock.Setup(s => s.GetSalesScoreByDateAsync(testDate, testDate))
            .ReturnsAsync(new Dictionary<string, int>());

        // Act
        var result = await _service.GetTopProductByDayAsync(testDate);

        // Assert
        Assert.Equal("No Product Found", result.Name);
        Assert.Equal("16/02/2026", result.Date);
    }

    [Fact]
    public async Task GetTopProductByDayAsync_ShouldHandleTieBreakerByAlphabeticalOrder()
    {
        // Arrange
        var testDate = new DateTime(2026, 2, 16);
        var scores = new Dictionary<string, int> { { "P1", 10 }, { "P2", 10 } };

        var products = new List<Product>
        {
            new Product { Id = "P1", Name = "Banana" },
            new Product { Id = "P2", Name = "Apple" }
        };

        _salesServiceMock.Setup(s => s.GetSalesScoreByDateAsync(testDate, testDate))
            .ReturnsAsync(scores);
        _productRepoMock.Setup(r => r.GetByIdsAsync(It.IsAny<List<string>>()))
            .ReturnsAsync(products);

        // Act
        var result = await _service.GetTopProductByDayAsync(testDate);

        // Assert
        Assert.Equal("Apple", result.Name);
    }

    [Fact]
    public async Task GetTopProductInPastThreeDaysAsync_ShouldCalculateCorrectDateRange()
    {
        // Arrange
        var endDate = new DateTime(2026, 2, 16);
        var startDate = endDate.AddDays(-2);

        _salesServiceMock.Setup(s => s.GetSalesScoreByDateAsync(startDate, endDate))
            .ReturnsAsync(new Dictionary<string, int> { { "P3", 5 } });
        _productRepoMock.Setup(r => r.GetByIdsAsync(It.IsAny<List<string>>()))
            .ReturnsAsync(new List<Product> { new Product { Id = "P3", Name = "Carrot" } });

        // Act
        var result = await _service.GetTopProductInPastThreeDaysAsync(endDate);

        // Assert
        Assert.Equal("14/02/2026 - 16/02/2026", result.Date);
        Assert.Equal("Carrot", result.Name);
        _salesServiceMock.Verify(s => s.GetSalesScoreByDateAsync(startDate, endDate), Times.Once);
    }
}