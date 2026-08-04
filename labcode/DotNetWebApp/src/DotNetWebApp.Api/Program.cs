using DotNetWebApp.Api.Contracts;
using DotNetWebApp.Api.Models;
using DotNetWebApp.Api.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Register one ProductStore for the lifetime of the application.
// All incoming requests therefore use the same in-memory product list.
builder.Services.AddSingleton<ProductStore>();

// Enables automatic validation for Minimal API parameters in .NET 10.
// The DataAnnotations placed on CreateProductRequest are checked before
// the POST handler is executed.
builder.Services.AddValidation();

WebApplication app = builder.Build();

/// <summary>
/// Returns basic information about this running application.
/// </summary>
app.MapGet(
        "/",
        (IHostEnvironment environment) =>
        {
            var applicationInformation = new
            {
                applicationName = "Product API",
                environmentName = environment.EnvironmentName,
                machineName = Environment.MachineName
            };

            return Results.Ok(applicationInformation);
        })
    .WithName("GetApplicationInformation")
    .Produces(StatusCodes.Status200OK);

/// <summary>
/// Returns every product currently held in memory.
/// </summary>
app.MapGet(
        "/products",
        (ProductStore store) =>
        {
            IReadOnlyList<Product> products = store.GetAll();

            return Results.Ok(products);
        })
    .WithName("GetProducts")
    .Produces<IReadOnlyList<Product>>(StatusCodes.Status200OK);

/// <summary>
/// Returns the product whose ID appears in the URL.
/// </summary>
app.MapGet(
        "/products/{id:int}",
        (int id, ProductStore store) =>
        {
            Product? product = store.GetById(id);

            return product is null
                ? Results.NotFound(new
                {
                    message = $"No product with ID {id} was found."
                })
                : Results.Ok(product);
        })
    .WithName("GetProductById")
    .Produces<Product>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

/// <summary>
/// Accepts a JSON request and creates a new product.
/// </summary>
app.MapPost(
        "/products",
        (CreateProductRequest request, ProductStore store) =>
        {
            // Validation attributes have already checked the incoming model.
            Product createdProduct = store.Add(request);

            // Return:
            //   HTTP 201 Created
            //   Location: /products/{new ID}
            //   The newly created product as JSON
            return Results.Created(
                $"/products/{createdProduct.Id}",
                createdProduct);
        })
    .WithName("CreateProduct")
    .Accepts<CreateProductRequest>("application/json")
    .Produces<Product>(StatusCodes.Status201Created)
    .ProducesValidationProblem();

app.Run();

// WebApplicationFactory needs access to the generated Program class.
// This declaration does not change the application's runtime behavior.
public partial class Program;