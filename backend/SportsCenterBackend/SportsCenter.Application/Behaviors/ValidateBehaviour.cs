using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace SportsCenter.Application.Behaviors;

internal class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest> _validator;

    public ValidationBehavior(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public ValidationBehavior()
    {
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator == null) return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResult = await _validator.ValidateAsync(context, cancellationToken);

        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        return await next();
    }
}