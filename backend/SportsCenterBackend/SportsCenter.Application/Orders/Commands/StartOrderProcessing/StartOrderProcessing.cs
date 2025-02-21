using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Orders.Commands.StartOrderProcessing
{
    public sealed record StartOrderProcessing : ICommand<Unit>
    {
        public int OrderId { get; set; }
        public StartOrderProcessing(int orderId)
        {
            OrderId = orderId;
        }
    }
}
