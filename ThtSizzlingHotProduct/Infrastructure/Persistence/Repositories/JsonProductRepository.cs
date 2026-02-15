using System.Text.Json;
using ThtSizzlingHotProduct.Domain.Entities;
using ThtSizzlingHotProduct.Domain.Interfaces;

namespace ThtSizzlingHotProduct.Infrastructure.Persistence.Repositories;

public class JsonProductRepository : IProductRepository
{
    private readonly string _inputFolderPath;

    public JsonProductRepository(string inputFolderPath)
    {
        _inputFolderPath = inputFolderPath ?? throw new ArgumentNullException(nameof(inputFolderPath));
    }

    public async Task<List<Product>> GetByIdsAsync(List<string> productIds)
    {
        if (productIds == null || !productIds.Any())
        {
            return new List<Product>();
        }

        var allProducts = await LoadAllProductsFromInputFileAsync();
        var selectedProducts = allProducts.Where(p => productIds.Contains(p.Id)).ToList();
        return selectedProducts;
    }

    private async Task<List<Product>> LoadAllProductsFromInputFileAsync()
    {
        var productsFilePath = Path.Combine(_inputFolderPath, "products.json");

        if (!File.Exists(productsFilePath))
        {
            throw new FileNotFoundException($"Products file not found: {productsFilePath}");
        }

        var json = await File.ReadAllTextAsync(productsFilePath);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var products = JsonSerializer.Deserialize<List<Product>>(json, options);

        return products ?? new List<Product>();
    }
}