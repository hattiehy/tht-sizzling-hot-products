using ThtSizzlingHotProduct.Application.Interfaces;
using ThtSizzlingHotProduct.Application.Models;
using ThtSizzlingHotProduct.Domain.Interfaces;

namespace ThtSizzlingHotProduct.Application.Services;

public class SizzlingProductService : ISizzlingProductService
{
    private readonly IProcessSalesService _processSalesService;
    private readonly IProductRepository _productRepository;

    public SizzlingProductService(IProcessSalesService processSalesService, IProductRepository productRepository)
    {
        _processSalesService = processSalesService ?? throw new ArgumentNullException(nameof(processSalesService));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task<TopProductDto> GetTopProductByDayAsync(DateTime date)
    {
        var topProductName = await GetTopProductAsync(date, date);
        return new TopProductDto(date.ToString("dd/MM/yyyy"), topProductName);
    }

    public async Task<TopProductDto> GetTopProductInPastThreeDaysAsync(DateTime endDate)
    {
        var startDate = endDate.AddDays(-2);
        var periodStr = $"{startDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy}";
        var topProductName = await GetTopProductAsync(startDate, endDate);
        return new TopProductDto(periodStr, topProductName);
    }

    private async Task<string> GetTopProductAsync(DateTime start, DateTime end)
    {
        const string defaultMessage = "No Product Found";

        var salesScores = await _processSalesService.GetSalesScoreByDateAsync(start, end);

        if (salesScores == null || !salesScores.Any())
        {
            return defaultMessage;
        }

        var maxScore = salesScores.Values.Max();
        var topProductIds = salesScores.Where(s => s.Value == maxScore)
            .Select(k => k.Key)
            .ToList();

        var products = await _productRepository.GetByIdsAsync(topProductIds);

        var topProduct = products.OrderBy(p => p.Name).FirstOrDefault();
        return topProduct?.Name ?? defaultMessage;
    }
}