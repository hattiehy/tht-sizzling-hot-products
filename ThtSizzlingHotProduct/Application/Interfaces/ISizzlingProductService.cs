using ThtSizzlingHotProduct.Application.Models;

namespace ThtSizzlingHotProduct.Application.Interfaces;

public interface ISizzlingProductService
{
    public Task<TopProductDto> GetTopProductByDayAsync(DateTime date);

    public Task<TopProductDto> GetTopProductInPastThreeDaysAsync(DateTime date);
}