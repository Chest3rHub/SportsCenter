using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.OrdersExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Orders.Commands.UpdateOrderPickUpDate
{
    internal sealed class UpdateOrderPickUpDateHandler : IRequestHandler<UpdateOrderPickUpDate, Unit>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateOrderPickUpDateHandler(IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor)
        {
            _orderRepository = orderRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(UpdateOrderPickUpDate request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int employeeId))
            {
                throw new UnauthorizedAccessException("You must be logged in to process an order.");
            }

            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId, cancellationToken);

            if (order == null)
            {
                throw new OrderNotFoundException(request.OrderId);
            }

            if (order.PracownikId != employeeId)
            {
                throw new UnauthorizedAccessException("You can only process orders assigned to you.");
            }

            if (order.Status != "Zrealizowane")
            {
                throw new WrongOrderStatusException("Zrealizowane");
            }

            order.DataOdbioru = request.PickUpDate;
            await _orderRepository.UpdateOrderAsync(order, cancellationToken);

            return Unit.Value;
        }
    }
}
