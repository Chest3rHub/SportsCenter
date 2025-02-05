using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reviews.Commands.AddReview
{
    public sealed record AddReview : ICommand<Unit>
    {
        public int ScheduleActivitiesClientId { get; set; }
        public string Description { get; set; }
        public int Stars { get; set; }
        public int ClientId { get; set; }
    }
}
