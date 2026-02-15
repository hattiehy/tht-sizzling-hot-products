using ThtSizzlingHotProduct.Domain.Entities;

namespace ThtSizzlingHotProduct.Domain.Interfaces;

public interface IProductRepository
{
    public Task<List<Product>> GetByIdsAsync(List<string> productId);
}