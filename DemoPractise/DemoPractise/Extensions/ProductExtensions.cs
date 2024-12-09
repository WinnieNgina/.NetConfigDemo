using DemoPractise.Models;
using DemoPractise.Records.Product;

namespace DemoPractise.Extensions;

public static class ProductExtensions
{
    public static ProductRecord ToProductRecord(this Product product)
    {
        if (product == null) return null;

        return new ProductRecord(
            product.ProductId,
            product.ProductName,
            product.ProductDescription,
            product.Price
        );
    }
}
