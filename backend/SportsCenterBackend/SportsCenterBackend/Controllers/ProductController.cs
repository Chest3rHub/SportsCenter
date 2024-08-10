using Microsoft.AspNetCore.Mvc;
using SportsCenterBackend.Entities;
using SportsCenterBackend.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using SportsCenterBackend.DTOs;

namespace SportsCenterBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductDbService _productDbService;

        public ProductController(IProductDbService productDbService)
        {
            _productDbService = productDbService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await _productDbService.GetProductsAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductWithoutIdDTO productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Produkt nie może być null");
            }

            var createdProduct = await _productDbService.AddProductAsync(productDto);
            return CreatedAtAction(nameof(GetProducts), new { id = createdProduct.ProduktId }, productDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productDbService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}