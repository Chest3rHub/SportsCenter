using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Wlasciciel")]
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
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        [Authorize(Roles = "Wlasciciel,Pracownik administracyjny")]
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
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        [Authorize(Roles = "Wlasciciel")]
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
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        [Authorize(Roles = "Wlasciciel,Pracownik administracyjny,Klient")]
        [HttpGet("get-products")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await Mediator.Send(new GetProducts());
            return Ok(products);
        }

        [Authorize(Roles = "Klient")]
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
                return BadRequest(new { Message = ex.Message });
            }
            catch (NoAvaliableEmployeeException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        [Authorize(Roles = "Klient")]
        [HttpGet("client-cart")]
        public async Task<IActionResult> GetCartProducts()
        {
            var products = await Mediator.Send(new GetCartProducts());
            return Ok(products);
        }

        [Authorize(Roles = "Klient")]
        [HttpDelete("remove-cart-product")]
        public async Task<IActionResult> RemoveCartProduct([FromBody] RemoveCartProduct query)
        {
            try
            {
                await Mediator.Send(new RemoveCartProduct(query.ProductId, query.Quantity));
                return Ok("Product removed from cart successfully");
            }
            catch (NoActiveOrdersForCLientException ex)
            {
                return NotFound(ex.Message);
            }
            catch (NoOrderedProductsException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (NotEnoughProductInStockException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        [Authorize(Roles = "Klient")]
        [HttpPost("cart-product-purchase")]
        public async Task<IActionResult> BuyCartProduct(CancellationToken cancellationToken)
        {          

            var command = new BuyCartProduct();
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
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }
    }
}
