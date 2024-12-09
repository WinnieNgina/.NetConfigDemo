using DemoPractise.Interfaces;
using DemoPractise.Records.Product;
using Microsoft.AspNetCore.Mvc;

namespace DemoPractise.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductRecord>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetProducts()
    {
        var result = await _productService.GetProductsAsync();
        if (!result.Success) return StatusCode(result.StatusCode, result.Message);
        return Ok(result.Data);
    }
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductRecord), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetProduct(int id)
    {
        var result = await _productService.GetProductAsync(id);
        if (!result.Success) return StatusCode(result.StatusCode, result.Message);
        return Ok(result.Data);
    }
    [HttpPost]
    [ProducesResponseType(typeof(ProductRecord), 201)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRecord createProductRecord)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _productService.AddProductAsync(createProductRecord);
        if (!result.Success) return StatusCode(result.StatusCode, result.Message);
        return CreatedAtAction(nameof(GetProduct), new { id = result.Data.ProductId }, result.Data);
    }
    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]

    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductRecord productRecord)
    {
        if (productRecord.ProductId != id)
        {
            return BadRequest();
        }
        var result = await _productService.UpdateProductAsync(productRecord);
        if (!result.Success) return StatusCode(result.StatusCode, result.Message);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _productService.DeleteProductAsync(id);
        if (!result.Success) return StatusCode(result.StatusCode, result.Message);
        return NoContent();

    }
}
