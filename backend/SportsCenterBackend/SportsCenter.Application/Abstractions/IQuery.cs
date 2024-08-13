using MediatR;

namespace SportsCenter.Application.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>;