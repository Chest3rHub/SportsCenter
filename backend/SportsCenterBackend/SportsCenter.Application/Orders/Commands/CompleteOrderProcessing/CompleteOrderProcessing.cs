using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Orders.Commands.ProcessOrder
{
    public sealed record CompleteOrderProcessing : ICommand<Unit>
    {
        public int OrderId { get; set; }
        public CompleteOrderProcessing(int orderId)
        {
            OrderId = orderId;
        }
    }
}
