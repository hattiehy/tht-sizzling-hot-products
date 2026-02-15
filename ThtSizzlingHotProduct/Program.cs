using ThtSizzlingHotProduct.Application.Interfaces;
using ThtSizzlingHotProduct.Application.Services;
using ThtSizzlingHotProduct.Domain.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ThtSizzlingHotProduct.Application.Models;
using ThtSizzlingHotProduct.Infrastructure.Persistence.Repositories;

namespace ThtSizzlingHotProduct;

/// <summary>
/// Application Entry Point
/// This is the Composition Root where all dependencies are configured
/// </summary>
class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║  Sizzling Hot Product Calculator          ║");
            Console.WriteLine("║  Using Microsoft DI Container              ║");
            Console.WriteLine("╚════════════════════════════════════════════╝\n");

            // ════════════════════════════════════════════════════════
            // BUILD DEPENDENCY INJECTION CONTAINER
            // ════════════════════════════════════════════════════════

            var host = CreateHostBuilder(args).Build();

            // ════════════════════════════════════════════════════════
            // RUN THE APPLICATION
            // ════════════════════════════════════════════════════════

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            // Get the main service from DI container
            var sizzlingProductService = services.GetRequiredService<ISizzlingProductService>();

            // Execute business logic
            await RunApplication(sizzlingProductService);

            Console.WriteLine("✅ Application completed successfully!\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            Environment.Exit(1);
        }
    }

    /// <summary>
    /// Create and configure the host with dependency injection
    /// This is where all services are registered
    /// </summary>
    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Get input folder path
                var inputPath = FindInputFolder();
                Console.WriteLine($"📂 Data folder: {inputPath}\n");

                // ════════════════════════════════════════════════════
                // REGISTER SERVICES IN DI CONTAINER
                // ════════════════════════════════════════════════════

                // Infrastructure Layer - Repositories
                // Singleton: One instance shared across the application
                services.AddSingleton<IOrderRepository>(provider =>
                    new JsonOrderRepository(inputPath));

                services.AddSingleton<IProductRepository>(provider =>
                    new JsonProductRepository(inputPath));

                // Application Layer - Services
                // Transient: New instance created each time it's requested
                services.AddTransient<IProcessSalesService, ProcessSalesService>();
                services.AddTransient<ISizzlingProductService, SizzlingProductService>();
            });

    /// <summary>
    /// Main application logic
    /// </summary>
    static async Task RunApplication(ISizzlingProductService sizzlingProductService)
    {
        Console.WriteLine("⏳ Calculating sizzling hot products...\n");

        // Dates to analyze (as per requirements)
        var day1 = new DateTime(2021, 7, 19);
        var day2 = new DateTime(2021, 7, 20);
        var day3 = new DateTime(2021, 7, 21);

        // Calculate results
        var result1 = await sizzlingProductService.GetTopProductByDayAsync(day1);
        var result2 = await sizzlingProductService.GetTopProductByDayAsync(day2);
        var result3 = await sizzlingProductService.GetTopProductByDayAsync(day3);
        var periodResult = await sizzlingProductService.GetTopProductInPastThreeDaysAsync(day3);

        // Display results
        PrintResults(result1, result2, result3, periodResult);
    }

    /// <summary>
    /// Print results in a formatted table
    /// </summary>
    static void PrintResults(
        TopProductDto result1,
        TopProductDto result2,
        TopProductDto result3,
        TopProductDto periodResult)
    {
        Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                         RESULTS                                              ║");
        Console.WriteLine("╠══════════════════════════════════════════════════════════════════════════════╣");
        Console.WriteLine("║ Date or Period          │ Top Sizzling Hot Product                           ║");
        Console.WriteLine("╠═════════════════════════╪════════════════════════════════════════════════════╣");

        PrintResult(result1);
        PrintResult(result2);
        PrintResult(result3);

        Console.WriteLine("╠═════════════════════════╪════════════════════════════════════════════════════╣");

        PrintResult(periodResult);

        Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝\n");
    }

    /// <summary>
    /// Print a single result row
    /// </summary>
    static void PrintResult(TopProductDto result)
    {
        var period = result.Date.PadRight(23);
        var product = result.Name;

        Console.WriteLine($"║ {period} │ {product.PadRight(50)} ║");
    }

    static string FindInputFolder()
    {
        var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var baseDataPath = Path.Combine(projectRoot, "inputs");
        if (Directory.Exists(baseDataPath))
            return baseDataPath;

        throw new DirectoryNotFoundException(
            "Could not find 'inputs' folder. Please ensure it exists in the project root.");
    }
}