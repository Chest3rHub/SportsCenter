using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Activities.Commands.PayForActivity
{
    public sealed record PayForActivity : ICommand<Unit>
    {
        public int ActivityId { get; set; }

        public PayForActivity(int activityId)
        {
            ActivityId = activityId;
        }
    }
}
