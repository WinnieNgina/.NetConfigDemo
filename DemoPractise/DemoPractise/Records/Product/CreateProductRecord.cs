using System.ComponentModel.DataAnnotations;

namespace DemoPractise.Records.Product;

public record CreateProductRecord
(
    [Required]
    [StringLength(100, ErrorMessage = "Product Name can't exceed 100 characters.")]
    string ProductName,

    [Required]
    [StringLength(500, ErrorMessage = "Product Description can't exceed 500 characters.")]
    string ProductDescription,

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    decimal Price
);
