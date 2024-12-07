using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Activities.Queries;

public sealed record GetSportActivityById(int SportActivityId) : IQuery<SportActivityDto>;
