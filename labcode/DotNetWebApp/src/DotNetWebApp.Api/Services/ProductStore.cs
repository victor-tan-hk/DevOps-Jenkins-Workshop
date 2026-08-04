using DotNetWebApp.Api.Contracts;
using DotNetWebApp.Api.Models;

namespace DotNetWebApp.Api.Services;

/// <summary>
/// Stores products in application memory.
///
/// The data exists only for the lifetime of the running application.
/// Restarting the application restores the original sample products.
/// </summary>
public sealed class ProductStore
{
    // A lock protects the list and ID generation when several requests
    // reach the application concurrently.
    private readonly object _syncRoot = new();

    // The three initial hardcoded products.
    private readonly List<Product> _products =
    [
        new Product(1, "Mechanical Keyboard", 12),
        new Product(2, "Wireless Mouse", 25),
        new Product(3, "USB-C Dock", 8)
    ];

    /// <summary>
    /// Returns a snapshot containing all current products.
    /// </summary>
    public IReadOnlyList<Product> GetAll()
    {
        lock (_syncRoot)
        {
            // Return a copy so callers cannot modify the internal list.
            return _products.ToList();
        }
    }

    /// <summary>
    /// Finds one product by its ID.
    /// </summary>
    public Product? GetById(int id)
    {
        lock (_syncRoot)
        {
            return _products.FirstOrDefault(product => product.Id == id);
        }
    }

    /// <summary>
    /// Creates and stores a product with the next available ID.
    /// </summary>
    public Product Add(CreateProductRequest request)
    {
        lock (_syncRoot)
        {
            // Find the highest existing ID and increment it.
            // DefaultIfEmpty protects against an empty collection.
            int nextId = _products
                .Select(product => product.Id)
                .DefaultIfEmpty(0)
                .Max() + 1;

            var product = new Product(
                nextId,
                request.Name.Trim(),
                request.Quantity);

            _products.Add(product);

            return product;
        }
    }
}