using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Activities.Commands.SignUpForActivity
{
    public sealed record SignUpForActivity : ICommand<Unit>
    {
        public int ActivityId { get; set; }
        public DateOnly SelectedDate { get; set; }
        public bool IsEquipmentIncluded { get; set; }

        public SignUpForActivity(int activityId, DateOnly selectedDate, bool isEquipmentIncluded)
        {
            ActivityId = activityId;
            SelectedDate = selectedDate;
            IsEquipmentIncluded = isEquipmentIncluded;
        }
    }
}
