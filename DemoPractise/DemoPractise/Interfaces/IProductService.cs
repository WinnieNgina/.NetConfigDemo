using DemoPractise.Records.Product;

namespace DemoPractise.Interfaces;

public interface IProductService
{
    Task<Result<IEnumerable<ProductRecord>>> GetProductsAsync();
    Task<Result<ProductRecord>> GetProductAsync(int productId);
    Task<Result<ProductRecord>> AddProductAsync(CreateProductRecord createProductRecord);
    Task<Result<bool>> UpdateProductAsync(ProductRecord productRecord);
    Task<Result<bool>> DeleteProductAsync(int productId);
}
