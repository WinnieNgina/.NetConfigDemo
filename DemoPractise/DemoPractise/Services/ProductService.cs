using DemoPractise.Data;
using DemoPractise.Extensions;
using DemoPractise.Interfaces;
using DemoPractise.Models;
using DemoPractise.Records.Product;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace DemoPractise.Services;

public class ProductService : IProductService
{
    private readonly DataContext _context;
    private readonly IPublishEndpoint _bus;
    public ProductService(DataContext context, IPublishEndpoint bus)
    {
        _context = context;
        _bus = bus;
    }
    public async Task<Result<ProductRecord>> AddProductAsync(CreateProductRecord createProductRecord)
    {
        try
        {
            var product = AddProduct(createProductRecord);
            var result = await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            var message = result.Entity.MapToCreated();
            await _bus.Publish(message);
            return new Result<ProductRecord> { Success = true, StatusCode = 201, Data = result.Entity.ToProductRecord() };

        }
        catch
        {
            return new Result<ProductRecord> { Success = false, StatusCode = 500, Message = "Server Error" };
        }
    }

    public async Task<Result<bool>> DeleteProductAsync(int productId)
    {
        try
        {
            if (productId <= 0) return new Result<bool> { Success = false, StatusCode = 400, Message = "Invalid ProductId" };
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return new Result<bool> { Success = false, StatusCode = 404, Message = "Product not found" };
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            var message = product.MapToDeleted();
            await _bus.Publish(message);
            return new Result<bool> { Success = true, StatusCode = 204, Message = "Product deleted successfully" };
        }
        catch
        {
            return new Result<bool> { Success = false, StatusCode = 500, Message = "Server Error" };
        }
    }

    public async Task<Result<ProductRecord>> GetProductAsync(int productId)
    {
        try
        {
            if (productId <= 0) return new Result<ProductRecord> { Success = false, StatusCode = 400, Message = "Invalid ProductId" };

            var product = await _context.Products.FindAsync(productId);
            if (product == null) return new Result<ProductRecord> { Success = false, StatusCode = 404, Message = "Product not found" };
            return new Result<ProductRecord> { Success = true, Data = product.ToProductRecord(), StatusCode = 200 };
        }
        catch
        {
            return new Result<ProductRecord>
            {
                Success = false,
                StatusCode = 500,
                Message = "Server Error"
            };
        }
    }

    public async Task<Result<IEnumerable<ProductRecord>>> GetProductsAsync()
    {
        try
        {
            var products = await _context.Products
                .Select(p => p.ToProductRecord())
                .AsNoTracking()
                .ToListAsync();

            return new Result<IEnumerable<ProductRecord>>
            {
                Success = true,
                Data = products,
                StatusCode = 200
            };
        }
        catch (Exception)
        {
            return new Result<IEnumerable<ProductRecord>>
            {
                Success = false,
                StatusCode = 500,
                Message = "Server Error"
            };
        }
    }

    public async Task<Result<bool>> UpdateProductAsync(ProductRecord productRecord)
    {
        try
        {
            if (productRecord.ProductId <= 0) return new Result<bool> { Success = false, StatusCode = 400, Message = "Invalid ProductId" };
            var product = await _context.Products.FindAsync(productRecord.ProductId);
            if (product == null) return new Result<bool> { Success = false, StatusCode = 404, Message = "Product not found" };
            product.ProductName = productRecord.ProductName;
            product.ProductDescription = productRecord.ProductDescription;
            product.Price = productRecord.Price;
            await _context.SaveChangesAsync();
            var message = product.MapToUpdated();
            await _bus.Publish(message);
            return new Result<bool> { Success = true, StatusCode = 204, Message = "Product updated successfully" };
        }
        catch
        {
            return new Result<bool> { Success = true, StatusCode = 500, Message = "Server Error" };
        }
    }
    private Product AddProduct(CreateProductRecord createProductRecord)
    {
        return new Product
        {
            ProductName = createProductRecord.ProductName,
            ProductDescription = createProductRecord.ProductDescription,
            Price = createProductRecord.Price,

        };
    }

}
