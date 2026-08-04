using System.ComponentModel.DataAnnotations;

namespace DotNetWebApp.Api.Contracts;

/// <summary>
/// Describes the JSON data required when creating a product.
/// </summary>
public sealed record CreateProductRequest
{
    /// <summary>
    /// Gets the product name.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public required string Name { get; init; }

    /// <summary>
    /// Gets the initial product quantity.
    /// </summary>
    [Range(0, int.MaxValue)]
    public int Quantity { get; init; }
}