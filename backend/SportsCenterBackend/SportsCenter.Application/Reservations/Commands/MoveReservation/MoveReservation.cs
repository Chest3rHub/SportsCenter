using MediatR;
using SportsCenter.Application.Abstractions;
using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Commands.MoveReservation
{
    public sealed record MoveReservation : ICommand<Unit>
    {
        public int ReservationId { get; set; }
        public DateTime NewStartTime { get; set; }
        public DateTime NewEndTime { get; set; }

        public MoveReservation(int reservationId, DateTime newStartTime, DateTime newEndTime)
        {
            ReservationId = reservationId;
            NewStartTime = newStartTime;
            NewEndTime = newEndTime;
        }
    }
}
