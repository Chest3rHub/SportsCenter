using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Activities.Commands.CancelSportActivity
{
    public sealed record CancelSportActivity : ICommand<Unit>
    {
        public int SportActivityId { get; set; }
        public DateOnly ActivityDate {  get; set; }

        public CancelSportActivity(int sportActivityId, DateOnly activityDate)
        {
            SportActivityId = sportActivityId;
            ActivityDate = activityDate;
        }
    }
}
