using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Clients.Commands.AddDiscount
{
    public sealed record AddDiscount : ICommand<Unit>
    {
        public int ClientId { get; set; }
        public int? ActivityDiscount { get; set; }
        public int? ProductDiscount { get; set; }
    }
}
