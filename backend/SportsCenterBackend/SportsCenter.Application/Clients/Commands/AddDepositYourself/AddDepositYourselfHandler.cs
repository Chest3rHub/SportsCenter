using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SportsCenter.Application.Clients.Commands.AddDepositYourself;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Core.Repositories;
using Stripe;
using Stripe.Checkout;

//rozwiazanie bezfrontendowe tymczasowe wpisac w swaggerze w StripeToken "tok_visa"
public class AddDepositYourselfHandler : IRequestHandler<AddDepositYourself, Unit>
{
    private readonly IClientRepository _clientRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly StripeSettings _stripeSettings;

    public AddDepositYourselfHandler(
        IClientRepository clientRepository,
        IHttpContextAccessor httpContextAccessor,
        IOptions<StripeSettings> stripeSettings)
    {
        _clientRepository = clientRepository;
        _httpContextAccessor = httpContextAccessor;
        _stripeSettings = stripeSettings.Value;
    }

    public async Task<Unit> Handle(AddDepositYourself request, CancellationToken cancellationToken)
    {
       
        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            throw new UnauthorizedAccessException("You are not authorized to perform this action.");
        }

        var client = await _clientRepository.GetClientByIdAsync(userId, cancellationToken);
        if (client == null)
        {
            throw new ClientWithGivenIdNotFoundException(userId);
        }

        var stripeToken = request.StripeToken; 
        if (string.IsNullOrEmpty(stripeToken))
        {
            throw new ArgumentException("Stripe token cannot be null or empty.");
        }

        var paymentMethodOptions = new PaymentMethodCreateOptions
        {
            Type = "card",
            Card = new PaymentMethodCardOptions
            {
                Token = stripeToken 
            }
        };

        var paymentMethodService = new PaymentMethodService();
        var paymentMethod = await paymentMethodService.CreateAsync(paymentMethodOptions);

        var session = await CreateCheckoutSession(request.Deposit);

        client.Saldo += request.Deposit;
        await _clientRepository.UpdateClientAsync(client, cancellationToken);

        return Unit.Value;
    }

    private async Task<Session> CreateCheckoutSession(decimal depositAmount)
    {
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "pln",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Deposit"
                        },
                        UnitAmount = (long)(depositAmount * 100) 
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment",
            SuccessUrl = "https://your-website.com/success",
            CancelUrl = "https://your-website.com/cancel",
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(options);

        return session;
    }
}
