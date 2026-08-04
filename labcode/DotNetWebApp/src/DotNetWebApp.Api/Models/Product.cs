namespace DotNetWebApp.Api.Models;

/// <summary>
/// Represents a product stored by the application.
/// </summary>
/// <param name="Id">The unique numeric identifier of the product.</param>
/// <param name="Name">The product's display name.</param>
/// <param name="Quantity">The number of units currently available.</param>
public sealed record Product(
    int Id,
    string Name,
    int Quantity);