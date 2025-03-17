using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Clients.Commands.UpdateDiscount
{
    public sealed record UpdateDiscount : ICommand<Unit>
    {
        public string ClientEmail { get; set; }
        public int? ActivityDiscount { get; set; }
        public int? ProductDiscount { get; set; }

        public UpdateDiscount(string clientEmail, int activityDiscount, int productDiscount)
        {
            ClientEmail = clientEmail;
            ActivityDiscount = activityDiscount;
            ProductDiscount = productDiscount;
        }
    }
}
