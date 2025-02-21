using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.OrdersExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Orders.Commands.ProcessOrder
{
    internal sealed class CompleteOrderProcessingHandler : IRequestHandler<CompleteOrderProcessing, Unit>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompleteOrderProcessingHandler(IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor)
        {
            _orderRepository = orderRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(CompleteOrderProcessing request, CancellationToken cancellationToken)
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

            if (order.Status != "W realizacji")
            {
                throw new WrongOrderStatusException("W realizacji");
            }

            order.Status = "Zrealizowane";
            order.DataRealizacji = DateOnly.FromDateTime(DateTime.Now);
            await _orderRepository.UpdateOrderAsync(order, cancellationToken);

            return Unit.Value;
        }
    }
}
