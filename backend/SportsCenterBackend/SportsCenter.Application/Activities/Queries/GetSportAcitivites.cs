using MediatR;
using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Activities.Queries;

public sealed record GetSportActivities : IQuery<IEnumerable<SportActivityDto>>;
