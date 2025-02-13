using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Commands.UpdateReservation
{
    public sealed record UpdateReservation : ICommand<Unit>
    {
        public int ReservationId { get; set; }
        public int TrainerId { get; set; }
    }
}
