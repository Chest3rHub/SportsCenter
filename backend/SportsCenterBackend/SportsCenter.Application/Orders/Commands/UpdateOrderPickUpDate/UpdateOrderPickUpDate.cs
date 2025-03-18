using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Orders.Commands.UpdateOrderPickUpDate
{
    public sealed record UpdateOrderPickUpDate : ICommand<Unit>
    {
        public int OrderId { get; set; }  
        public DateOnly PickUpDate { get; set; }
        public UpdateOrderPickUpDate(int orderId, DateOnly pickUpDate)
        {
            OrderId = orderId;
            PickUpDate = pickUpDate;
        }
    }
}
