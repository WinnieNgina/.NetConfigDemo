using DemoPractise.Models;
using DemoPractise.Records.Product;
using MessagingContracts;

namespace DemoPractise.Extensions;

public static class ProductsMapper
{
    public static ProductCreated MapToCreated(this Product product)
    {
        return new ProductCreated(
            product.ProductId,
            product.ProductName,
            product.ProductDescription,
            product.Price
        );
    }
    public static ProductUpdated MapToUpdated(this Product product)
    {
        return new ProductUpdated(
            product.ProductId,
            product.ProductName,
            product.ProductDescription,
            product.Price
        );
    }
    public static ProductDeleted MapToDeleted(this Product product)
    {
        return new ProductDeleted(
            product.ProductId,
            product.ProductName,
            product.ProductDescription,
            product.Price
        );
    }
}
