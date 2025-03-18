using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.ProductsExceptions;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Application.Reservations.Commands.RemoveReservation;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Products.Commands.RemoveProduct
{
    internal sealed class RemoveProductHandler : IRequestHandler<RemoveProduct, Unit>
    {
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RemoveProductHandler(IProductRepository productRepository, IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(RemoveProduct request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductByIdAsync(request.ProductId, cancellationToken);

            if (product == null)
            {
                throw new ProductNotFoundException(request.ProductId);
            }

            await _productRepository.DeleteProductAsync(product, cancellationToken);

            return Unit.Value;
        }
    }
}
