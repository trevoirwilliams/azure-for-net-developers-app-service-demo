using ProductCatalog.Api.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string releaseVersion = "1.0.0";

Product[] products =
[
    new(1, "Wireless Mouse", "Accessories", 24.99m, true),
    new(2, "Mechanical Keyboard", "Accessories", 79.99m, true),
    new(3, "USB-C Hub", "Accessories", 39.99m, false),
    new(4, "Laptop Stand", "Office", 34.99m, true),
    new(5, "Desk Lamp", "Office", 29.99m, true)
];

app.MapGet("/api/products", (IConfiguration configuration, ILogger<Program> logger) =>
{
    var mode = configuration["Catalog:Mode"];
    if (mode is not ("standard" or "maintenance"))
    {
        logger.LogWarning(
            "CATALOG_MODE_INVALID. Catalog:Mode must be standard or maintenance.");

        return Results.Problem(
            title: "The product catalogue is temporarily unavailable.",
            statusCode: StatusCodes.Status503ServiceUnavailable);
    }

    return Results.Ok(new
    {
        storeName = configuration["Catalog:StoreName"],
        mode,
        products
    });
});

app.MapGet("/api/info", (IConfiguration configuration, IWebHostEnvironment environment) =>
    Results.Ok(new
    {
        application = "product-catalog-api",
        releaseVersion,
        storeName = configuration["Catalog:StoreName"],
        environment = environment.EnvironmentName
    }));

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run();
