using MediatR;

namespace SportsCenter.Application.Abstractions;

public interface ICommand<out TResponse> : IRequest<TResponse>;