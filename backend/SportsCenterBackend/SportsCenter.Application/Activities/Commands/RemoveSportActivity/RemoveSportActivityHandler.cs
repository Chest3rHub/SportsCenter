using MediatR;
using SportsCenter.Core.Repositories;
using SportsCenter.Application.Exceptions;
using SportsCenter.Application.Exceptions.SportActivitiesException;

namespace SportsCenter.Application.Activities.Commands.RemoveSportActivity;

internal sealed class RemoveSportActivityHandler : IRequestHandler<RemoveSportActivity, Unit>
{
    private readonly ISportActivityRepository _activityRepository;

    public RemoveSportActivityHandler(ISportActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public async Task<Unit> Handle(RemoveSportActivity request, CancellationToken cancellationToken)
    { 
        var activity = await _activityRepository.GetSportActivityByIdAsync(request.SportActivityId, cancellationToken);

        if (activity == null)
            throw new SportActivityNotFoundException(request.SportActivityId);

        await _activityRepository.RemoveSportActivityAsync(activity, cancellationToken);

        return Unit.Value;
    }
}