using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Exceptions.OrdersExceptions;
using SportsCenter.Application.Orders.Commands.ProcessOrder;
using SportsCenter.Application.Orders.Commands.StartOrderProcessing;
using SportsCenter.Application.Orders.Commands.UpdateOrderPickUpDate;
using SportsCenter.Application.Orders.Queries.GetClientOrders;
using SportsCenter.Application.Orders.Queries.GetOrdersToProcess;

namespace SportsCenter.Api.Controllers
{
    [Route("[controller]")]
    public class OrdersController : BaseController
    {
        public OrdersController(IMediator mediator) : base(mediator)
        {
        }

        [Authorize(Roles = "Pracownik administracyjny")]
        [HttpGet("Get-orders-to-process")]
        public async Task<IActionResult> GetOrdersToProcessAsync()
        {
            return Ok(await Mediator.Send(new GetOrdersToProcess()));
        }

        [Authorize(Roles = "Pracownik administracyjny")]
        [HttpPut(("Complete-order-processing"))]
        public async Task<IActionResult> CompleteOrderProcessingAsync([FromBody] CompleteOrderProcessing processOrder)
        {
            try
            {
                await Mediator.Send(processOrder);
                return NoContent();
            }
            catch (OrderNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (WrongOrderStatusException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [Authorize(Roles = "Pracownik administracyjny")]
        [HttpPut(("Start-order-processing"))]
        public async Task<IActionResult>StartOrderProcessingAsync([FromBody] StartOrderProcessing processOrder)
        {
            try
            {
                await Mediator.Send(processOrder);
                return NoContent();
            }
            catch (OrderNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (OrderAlreadyProcessedException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [Authorize(Roles = "Klient")]
        [HttpGet("Get-orders")]
        public async Task<IActionResult> GetOrdersAsync()
        {
            return Ok(await Mediator.Send(new GetClientOrders()));
        }

        [Authorize(Roles = "Pracownik administracyjny")]
        [HttpPut(("Update-order-pick-up-date"))]
        public async Task<IActionResult> UpdateOrderPickUpDateAsyncc([FromBody] UpdateOrderPickUpDate order)
        {
            try
            {
                await Mediator.Send(order);
                return NoContent();
            }
            catch (OrderNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (WrongOrderStatusException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }
    }
}
