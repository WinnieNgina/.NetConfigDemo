using Carter;
using DemoPractise.Interfaces;
using DemoPractise.Records.Product;
using FluentValidation;

namespace DemoPractise.Controllers;

// https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-9.0&tabs=visual-studio
// https://learn.microsoft.com/en-us/training/modules/build-web-api-aspnet-core/?source=recommendations
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/responses?view=aspnetcore-9.0#typedresults-vs-results
public class ProductsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/product/");

        group.MapPost("", CreateProduct)
            .Produces<ProductRecord>(201)
            .Produces(400)
            .Produces(500)
            .WithName(nameof(CreateProduct));

        group.MapGet("", GetProducts)
            .Produces<IEnumerable<ProductRecord>>(200)
            .Produces(500)
            .WithName(nameof(GetProducts));

        group.MapGet("{id:int}", GetProduct)
            .Produces<ProductRecord>(200)
            .Produces(400)
            .Produces(404)
            .Produces(500)
            .WithName(nameof(GetProduct));

        group.MapPatch("{id:int}", UpdateProduct)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithName(nameof(UpdateProduct));
        group.MapDelete("{id:int}", DeleteProduct)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithName(nameof(DeleteProduct));
    }
    // public static void MapProductsEndpoints(this IEndpointRouteBuilder app)
   
    public static async Task<IResult> GetProducts(IProductService _productService)
    {
        var result = await _productService.GetProductsAsync();
        return result.Success ? TypedResults.Ok(result.Data) :
            TypedResults.Problem(statusCode: result.StatusCode, detail: result.Message);
    }

    public static async Task<IResult> GetProduct(int id, IProductService productService)
    {
        var result = await productService.GetProductAsync(id);
        return result.Success
            ? TypedResults.Ok(result.Data)
            : TypedResults.Problem(statusCode: result.StatusCode, detail: result.Message);
    }


    public static async Task<IResult> CreateProduct(CreateProductRecord createProductRecord, IValidator<CreateProductRecord> _validation, IProductService productService, LinkGenerator linkGenerator)
    {
        var validationResult = await _validation.ValidateAsync(createProductRecord);
        if (!validationResult.IsValid)
        {
            return TypedResults.BadRequest(validationResult.Errors.FirstOrDefault().ToString());
        }

        var result = await productService.AddProductAsync(createProductRecord);
        if (result.Success)
        {
            var url = linkGenerator.GetPathByName("GetProduct", new { id = result.Data.ProductId });
            return TypedResults.Created(url, result.Data);
        }
        return TypedResults.Problem(statusCode: result.StatusCode, detail: result.Message);
    }

    public static async Task<IResult> UpdateProduct(int id, ProductRecord productRecord, IProductService productService)
    {
        if (productRecord.ProductId != id)
        {
            return TypedResults.BadRequest();
        }
        var result = await productService.UpdateProductAsync(productRecord);
        return result.Success ?
             TypedResults.Ok(result.Message) :
             TypedResults.Problem(statusCode: result.StatusCode, detail: result.Message);
    }
    public static async Task<IResult> DeleteProduct(int id, IProductService productService)
    {
        var result = await productService.DeleteProductAsync(id);
        return result.Success ? TypedResults.Ok(result.Message) :
            TypedResults.Problem(statusCode: result.StatusCode, detail: result.Message);

    }
}
