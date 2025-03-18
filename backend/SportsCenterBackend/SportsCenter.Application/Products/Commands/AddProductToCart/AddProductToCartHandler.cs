using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Application.Exceptions.ProductsExceptions;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Products.Commands.AddProductToCart
{
    internal sealed class AddProductToCartHandler : IRequestHandler<AddProductToCart, Unit>
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddProductToCartHandler(IProductRepository productRepository, IOrderRepository orderRepository, IEmployeeRepository employeeRepository, IClientRepository clientRepository, IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _clientRepository = clientRepository;
            _employeeRepository = employeeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(AddProductToCart request, CancellationToken cancellationToken)
        {

            var userId = GetUserIdFromSession();
            if (userId == null)
            {
                throw new UnauthorizedAccessException("You cannot add a product to your cart without logging in.");
            }

            var product = await _productRepository.GetProductByIdAsync(request.ProductId, cancellationToken);
            if (product == null)
            {
                throw new ProductNotFoundException(request.ProductId);
            }

            var order = await _orderRepository.GetActiveOrderByUserIdAsync((int)userId, cancellationToken);
            if (order == null)
            {
                var pracownik = await _employeeRepository.GetEmployeeWithFewestOrdersAsync(cancellationToken);
                if (pracownik == null)
                {
                    throw new NoAvaliableEmployeeException();
                }
                order = new Zamowienie
                {
                    KlientId = (int)userId,
                    Data = DateOnly.FromDateTime(DateTime.UtcNow),
                    Status = "Koszyk",
                    PracownikId = pracownik.PracownikId,
                    ZamowienieProdukts = new List<ZamowienieProdukt>()
                };
                await _orderRepository.AddOrderAsync(order, cancellationToken);
            }

            var existingOrderProduct = await _orderRepository.GetOrderProductAsync(order.ZamowienieId, product.ProduktId, cancellationToken);

            int totalRequestedQuantity = (existingOrderProduct?.Liczba ?? 0) + request.Quantity;

            if (totalRequestedQuantity > product.LiczbaNaStanie)
            {
                throw new NotEnoughProductInStockException(request.ProductId);
            }

            if (existingOrderProduct != null)
            {
                existingOrderProduct.Liczba += request.Quantity;
                var cost = existingOrderProduct.Liczba * product.Koszt;

                var client = await _clientRepository.GetClientByIdAsync((int)userId, cancellationToken);
                if (client.ZnizkaNaProdukty.HasValue && client.ZnizkaNaProdukty.Value > 0)
                {
                    cost *= (1 - client.ZnizkaNaProdukty.Value / 100m);
                }

                existingOrderProduct.Koszt = cost;
                await _orderRepository.UpdateOrderProductAsync(existingOrderProduct, cancellationToken);
            }
            else
            {
                var cost = request.Quantity * product.Koszt;

                var client = await _clientRepository.GetClientByIdAsync((int)userId, cancellationToken);
                if (client.ZnizkaNaProdukty.HasValue && client.ZnizkaNaProdukty.Value > 0)
                {
                    cost *= (1 - client.ZnizkaNaProdukty.Value / 100m);
                }

                var newOrderProduct = new ZamowienieProdukt
                {
                    ZamowienieId = order.ZamowienieId,
                    ProduktId = product.ProduktId,
                    Liczba = request.Quantity,
                    Koszt = cost
                };
                await _orderRepository.AddOrderProductAsync(newOrderProduct, cancellationToken);
            }

            return Unit.Value;
        }
        private int? GetUserIdFromSession()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            return null;
        }

    }
}
