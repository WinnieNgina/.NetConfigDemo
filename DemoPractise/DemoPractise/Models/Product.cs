using System.ComponentModel.DataAnnotations;

namespace DemoPractise.Models;

public class Product
{
    public int ProductId { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "Product Name can't exceed 100 characters.")]
    public string ProductName { get; set; } = null!;
    [Required]

    [StringLength(500, ErrorMessage = "Product Description can't exceed 500 characters.")]
    public string ProductDescription { get; set; } = null!;
    public decimal Price { get; set; } = 0.00m;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
}
