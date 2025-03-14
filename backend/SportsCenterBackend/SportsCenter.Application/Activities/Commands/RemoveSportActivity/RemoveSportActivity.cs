using MediatR;
using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Activities.Commands.RemoveSportActivity
{
    public sealed record RemoveSportActivity : ICommand<Unit>
    {
        public int SportActivityId { get; set; }

        public RemoveSportActivity(int sportActivityId)
        {
            SportActivityId = sportActivityId;
        }
    }
}

