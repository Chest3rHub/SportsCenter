using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.ProductsExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Products.Commands.RemoveCartProduct
{
    internal sealed class RemoveCartProductHandler : IRequestHandler<RemoveCartProduct, Unit>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RemoveCartProductHandler(IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor)
        {
            _orderRepository = orderRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(RemoveCartProduct request, CancellationToken cancellationToken)
        {
            var userId = request.ClientId;

            var order = await _orderRepository.GetActiveOrderByUserIdAsync(userId, cancellationToken);
            if (order == null)
            {
                throw new NoActiveOrdersForCLientException(request.ClientId);
            }

            var orderProduct = await _orderRepository.GetOrderProductAsync(order.ZamowienieId, request.ProductId, cancellationToken);
            if (orderProduct == null)
            {
                throw new NoOrderedProductsException();
            }

            await _orderRepository.RemoveOrderProductAsync(orderProduct, cancellationToken);

            return Unit.Value;
        }
    }
}
