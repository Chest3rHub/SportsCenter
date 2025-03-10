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
        public string DayOfWeek { get; set; }
        public bool IsEquipmentIncluded { get; set; }

        public SignUpForActivity(int activityId, string dayOfWeek, bool isEquipmentIncluded)
        {
            ActivityId = activityId;
            DayOfWeek = dayOfWeek;
            IsEquipmentIncluded = isEquipmentIncluded;
        }
    }
}
