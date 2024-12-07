using MediatR;
using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Activities.Commands.RemoveSportActivity;
public sealed record RemoveSportActivity(int SportActivityId) : ICommand<Unit>;

