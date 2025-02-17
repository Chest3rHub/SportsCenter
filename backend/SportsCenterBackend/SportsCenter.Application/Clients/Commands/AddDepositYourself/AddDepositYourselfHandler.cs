using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SportsCenter.Application.Clients.Commands.AddDepositYourself;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Core.Repositories;
using Stripe;
using Stripe.Checkout;

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
        _stripeSettings = stripeSettings.Value;  // Wczytujemy konfigurację z DI
    }

    public async Task<Unit> Handle(AddDepositYourself request, CancellationToken cancellationToken)
    {
        // Inicjalizacja Stripe API
        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

        // Pobieramy userId z kontekstu HTTP
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

        // Sprawdzamy token Stripe, który został przesłany z frontendu
        var stripeToken = request.StripeToken; // Zakładając, że token jest przesyłany z frontendu
        if (string.IsNullOrEmpty(stripeToken))
        {
            throw new ArgumentException("Stripe token cannot be null or empty.");
        }

        // Tworzymy PaymentMethod za pomocą tokenu
        var paymentMethodOptions = new PaymentMethodCreateOptions
        {
            Type = "card",
            Card = new PaymentMethodCardOptions
            {
                Token = stripeToken // Używamy tokenu karty, a nie numeru karty
            }
        };

        var paymentMethodService = new PaymentMethodService();
        var paymentMethod = await paymentMethodService.CreateAsync(paymentMethodOptions);

        // Tworzymy sesję Checkout
        var session = await CreateCheckoutSession(request.Deposit);

        // Zwiększamy saldo klienta
        client.Saldo += request.Deposit;
        await _clientRepository.UpdateClientAsync(client, cancellationToken);

        // Możesz zwrócić URL sesji Stripe, aby użytkownik mógł zakończyć płatność
        return Unit.Value; // Możesz zwrócić session.Url, jeśli chcesz przekazać go klientowi
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
                        UnitAmount = (long)(depositAmount * 100)  // Stripe expects the amount in cents
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
