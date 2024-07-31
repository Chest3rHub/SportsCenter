using Microsoft.AspNetCore.Mvc;
using SportsCenterBackend.Entities;
using SportsCenterBackend.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IEnumerable<Produkt>>> GetProducts()
        {
            var products = await _productDbService.GetProductsAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Produkt product)
        {
            if (product == null)
            {
                return BadRequest("Produkt nie może być null");
            }

            await _productDbService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProducts), new { id = product.ProduktId }, product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productDbService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}