using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.ProductsExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Products.Commands.UpdateProduct
{
    internal sealed class UpdateProductHandler : IRequestHandler<UpdateProduct, Unit>
    {
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateProductHandler(IProductRepository productRepository, IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(UpdateProduct request, CancellationToken cancellationToken)
        {           
            var product = await _productRepository.GetProductByIdAsync(request.ProductId, cancellationToken);
            if (product == null)
            {
                throw new ProductNotFoundException(request.ProductId);
            }

            product.Nazwa = request.Name;
            product.Producent = request.Manufacturer;
            product.LiczbaNaStanie = request.StockQuantity;
            product.Koszt = request.Price;
            product.ZdjecieUrl = request.ImageUrl;

            await _productRepository.UpdateProductAsync(product, cancellationToken);

            return Unit.Value;
        }
    }
}
