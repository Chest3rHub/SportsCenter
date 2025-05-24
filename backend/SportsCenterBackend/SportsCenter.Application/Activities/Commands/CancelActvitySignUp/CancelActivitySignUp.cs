using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Activities.Commands.CancelActvitySignUp
{
    public sealed record CancelActivitySignUp : ICommand<Unit>
    {
        public int InstanceOfActivityId { get; set; } //to sie pobiera po kliknieciu na kafelek ze swojego grafiku
        public DateOnly SelectedDate { get; set; }

        public CancelActivitySignUp(int instanceOfActivityId, DateOnly selectedDate)
        {
            InstanceOfActivityId = instanceOfActivityId;
            SelectedDate = selectedDate;         
        }
    }
}
