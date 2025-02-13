using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Application.Exceptions.ProductsExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Products.Commands.BuyCartProduct
{
    internal sealed class BuyCartProductHandler : IRequestHandler<BuyCartProduct, Unit>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BuyCartProductHandler(IOrderRepository orderRepository, IProductRepository productRepository, IClientRepository clientRepository, IHttpContextAccessor httpContextAccessor)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _clientRepository = clientRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(BuyCartProduct request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("You cannot purchase a product into your cart without logging in.");
            }
        
            var client = await _clientRepository.GetClientByIdAsync(userId, cancellationToken);
            if (client == null)
            {
                throw new ClientWithGivenIdNotFoundException(userId);
            }

            var order = await _orderRepository.GetActiveOrderByUserIdAsync(userId, cancellationToken);
            if (order == null || !order.ZamowienieProdukts.Any())
            {
                throw new NoActiveOrdersForCLientException(userId);
            }
           
            var discount = await _clientRepository.GetProductDiscountForClientAsync(userId, cancellationToken);
         
            decimal totalCost = await _orderRepository.GetTotalOrderCostAsync(order.ZamowienieId, (decimal)discount, cancellationToken);        

            if (totalCost == 0m)
            {
                throw new NoOrderedProductsException();
            }

            if (client.Saldo < totalCost)
            {
                throw new NotEnoughFundsInAccountBalance();
            }
       
            client.Saldo -= totalCost;
            await _clientRepository.UpdateClientAsync(client, cancellationToken);
 
            order.Status = "W realizacji";
            await _orderRepository.UpdateOrderAsync(order, cancellationToken);

            foreach (var orderProduct in order.ZamowienieProdukts)
            {              
                var product = await _productRepository.GetProductByIdAsync(orderProduct.ProduktId, cancellationToken);
                if (product == null)
                {
                    throw new ProductNotFoundException(orderProduct.ProduktId);
                }

                if (product.LiczbaNaStanie < orderProduct.Liczba)
                {
                    throw new NotEnoughProductInStockException(orderProduct.ProduktId);
                }

                product.LiczbaNaStanie -= orderProduct.Liczba;
                await _productRepository.UpdateProductAsync(product, cancellationToken);
            }

            return Unit.Value;
        }


    }
}
