using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Activities.Commands.PayForClientActivity
{
    public sealed record PayForClientActivity : ICommand<Unit>
    {
        public int ActivityId { get; set; }
        public string ClientEmail { get; set; }

        public PayForClientActivity(int activityId, string clientEmail)
        {
            ActivityId = activityId;
            ClientEmail = clientEmail;
        }
    }
}
