using MediatR;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Application.Exceptions.ProductsExceptions;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Application.Products.Commands.AddProduct;
using SportsCenter.Application.Products.Commands.AddProductToCart;
using SportsCenter.Application.Products.Commands.BuyCartProduct;
using SportsCenter.Application.Products.Commands.RemoveCartProduct;
using SportsCenter.Application.Products.Commands.RemoveProduct;
using SportsCenter.Application.Products.Commands.UpdateProduct;
using SportsCenter.Application.Products.Queries.GetCartProducts;
using SportsCenter.Application.Products.Queries.GetProducts;
using SportsCenter.Application.Reservations.Commands.AddReservation;
using SportsCenter.Application.Reservations.Commands.RemoveReservation;

namespace SportsCenter.Api.Controllers
{
    [Route("[controller]")]
    public class ProductsController : BaseController
    {
        public ProductsController(IMediator mediator) : base(mediator)
        {
        }
        [HttpPost("Add-product")]
        public async Task<IActionResult> AddProduct([FromBody] AddProduct product)
        {
            var validationResults = new AddProductValidator().Validate(product);
            if (!validationResults.IsValid)
            {
                return BadRequest(validationResults.Errors);
            }

            try
            {
                await Mediator.Send(product);
                return Ok(new { Message = "Product added successfully" });
            }            
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }
        
        [HttpPut(("update-products"))]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProduct updateProduct)
        {
            var validationResults = new UpdateProductValidator().Validate(updateProduct);
            if (!validationResults.IsValid)
            {
                return BadRequest(validationResults.Errors);
            }

            try
            {
                await Mediator.Send(updateProduct);
                return NoContent();
            }
            catch (ProductNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }
        [HttpDelete("delete-products")]
        public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken)
        {
            try
            {
                var request = new RemoveProduct(id);
                await Mediator.Send(request, cancellationToken);
                return Ok("Product canceled successfully.");
            }
            catch (ProductNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [HttpGet("get-products")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await Mediator.Send(new GetProducts());
            return Ok(products);
        }

        [HttpPost("Add-product-to-cart")]
        public async Task<IActionResult> AddProductToCart([FromBody] AddProductToCart product)
        {
            var validationResults = new AddProductToCartValidator().Validate(product);
            if (!validationResults.IsValid)
            {
                return BadRequest(validationResults.Errors);
            }

            try
            {
                await Mediator.Send(product);
                return Ok(new { Message = "Product added to cart successfully" });
            }
            catch (ProductNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (NotEnoughProductInStockException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (NoAvaliableEmployeeException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [HttpGet("client-cart")]
        public async Task<IActionResult> GetCartProducts(int clientId)
        {
            var products = await Mediator.Send(new GetCartProducts(clientId));
            return Ok(products);
        }

        [HttpDelete("remove-cart-product")]
        public async Task<IActionResult> RemoveCartProduct([FromQuery] int userId, [FromQuery] int productId)
        {
            try
            {
                await Mediator.Send(new RemoveCartProduct(userId,productId));
                return Ok("Product removed from cart successfully");
            }
            catch (NoActiveOrdersForCLientException ex)
            {
                return NotFound(ex.Message);
            }
            catch (NoOrderedProductsException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }
        [HttpPost("cart-product-purchase")]
        public async Task<IActionResult> BuyCartProduct(CancellationToken cancellationToken)
        {
            var userId = 1;//tymczasowo  

            var command = new BuyCartProduct(userId);
            try
            {
                await Mediator.Send(command, cancellationToken);
                return Ok(new { message = "Purchase completed successfully" });
            }
            catch (ClientWithGivenIdNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (NoActiveOrdersForCLientException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (NoOrderedProductsException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (NotEnoughFundsInAccountBalance ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (ProductNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (NotEnoughProductInStockException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }
    }
}
