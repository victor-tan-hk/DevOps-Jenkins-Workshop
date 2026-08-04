using System.Net;
using System.Net.Http.Json;
using DotNetWebApp.Api.Contracts;
using DotNetWebApp.Api.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DotNetWebApp.Api.Tests;

/// <summary>
/// Integration tests for the Product API.
///
/// WebApplicationFactory starts the API inside the test process. Each test
/// sends real HTTP requests through ASP.NET Core routing, model binding,
/// validation and JSON serialization.
/// </summary>
public sealed class ProductApiTests :
    IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    /// <summary>
    /// Creates an HTTP client connected to the in-memory API server.
    /// </summary>
    public ProductApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetRoot_ReturnsApplicationInformation()
    {
        // Act: send a GET request to the root endpoint.
        HttpResponseMessage response = await _client.GetAsync("/");

        // Assert: the request should have succeeded.
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        ApplicationInformation? information =
            await response.Content.ReadFromJsonAsync<ApplicationInformation>();

        Assert.NotNull(information);
        Assert.Equal("Product API", information.ApplicationName);
        Assert.False(string.IsNullOrWhiteSpace(information.EnvironmentName));
        Assert.False(string.IsNullOrWhiteSpace(information.MachineName));
    }

    [Fact]
    public async Task GetProducts_ReturnsTheInitialThreeProducts()
    {
        // Act.
        HttpResponseMessage response = await _client.GetAsync("/products");

        // Assert.
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        List<Product>? products =
            await response.Content.ReadFromJsonAsync<List<Product>>();

        Assert.NotNull(products);
        Assert.Equal(3, products.Count);

        Assert.Contains(products, product => product.Id == 1);
        Assert.Contains(products, product => product.Id == 2);
        Assert.Contains(products, product => product.Id == 3);
    }

    [Fact]
    public async Task GetProductById_WithExistingId_ReturnsProduct()
    {
        // Act.
        HttpResponseMessage response = await _client.GetAsync("/products/2");

        // Assert.
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        Product? product =
            await response.Content.ReadFromJsonAsync<Product>();

        Assert.NotNull(product);
        Assert.Equal(2, product.Id);
        Assert.Equal("Wireless Mouse", product.Name);
        Assert.Equal(25, product.Quantity);
    }

    [Fact]
    public async Task GetProductById_WithUnknownId_ReturnsNotFound()
    {
        // Act.
        HttpResponseMessage response = await _client.GetAsync("/products/999");

        // Assert.
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PostProduct_WithValidJson_CreatesProduct()
    {
        // Arrange: the client sends only name and quantity.
        var request = new CreateProductRequest
        {
            Name = "Web Camera",
            Quantity = 17
        };

        // Act.
        HttpResponseMessage response =
            await _client.PostAsJsonAsync("/products", request);

        // Assert: REST creation convention is HTTP 201.
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        Product? createdProduct =
            await response.Content.ReadFromJsonAsync<Product>();

        Assert.NotNull(createdProduct);
        Assert.True(createdProduct.Id > 3);
        Assert.Equal("Web Camera", createdProduct.Name);
        Assert.Equal(17, createdProduct.Quantity);

        // The response should identify the new resource's URL.
        Assert.Equal(
            $"/products/{createdProduct.Id}",
            response.Headers.Location?.OriginalString);

        // Confirm that the product was actually stored.
        Product? retrievedProduct =
            await _client.GetFromJsonAsync<Product>(
                $"/products/{createdProduct.Id}");

        Assert.Equal(createdProduct, retrievedProduct);
    }

    [Fact]
    public async Task PostProduct_WithBlankName_ReturnsBadRequest()
    {
        // Arrange.
        var invalidRequest = new
        {
            name = "",
            quantity = 5
        };

        // Act.
        HttpResponseMessage response =
            await _client.PostAsJsonAsync("/products", invalidRequest);

        // Assert: the DataAnnotations validation should reject the input.
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostProduct_WithNegativeQuantity_ReturnsBadRequest()
    {
        // Arrange.
        var invalidRequest = new
        {
            name = "Invalid Product",
            quantity = -1
        };

        // Act.
        HttpResponseMessage response =
            await _client.PostAsJsonAsync("/products", invalidRequest);

        // Assert.
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    /// <summary>
    /// Test-only type used to deserialize the root endpoint response.
    /// JSON property matching is case-insensitive by default.
    /// </summary>
    private sealed record ApplicationInformation(
        string ApplicationName,
        string EnvironmentName,
        string MachineName);
}