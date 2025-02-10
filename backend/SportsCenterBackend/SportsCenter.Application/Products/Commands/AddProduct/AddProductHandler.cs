using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Products.Commands.AddProduct
{
    internal sealed class AddProductHandler : IRequestHandler<AddProduct, Unit>
    {
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddProductHandler(IProductRepository productRepository, IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(AddProduct request, CancellationToken cancellationToken)
        {
            var product = new Produkt
            {
                Nazwa = request.Name,
                Producent = request.Manufacturer,
                LiczbaNaStanie = request.StockQuantity,
                Koszt = request.Price,
                ZdjecieUrl = request.ImageUrl ?? string.Empty
            };
       
            await _productRepository.AddProductAsync(product, cancellationToken);

            return Unit.Value;
        }
    }
}
