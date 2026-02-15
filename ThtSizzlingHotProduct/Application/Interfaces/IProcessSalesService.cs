namespace ThtSizzlingHotProduct.Application.Interfaces;

public interface IProcessSalesService
{
    Task<Dictionary<string, int>> GetSalesScoreByDateAsync(DateTime start, DateTime end);
}