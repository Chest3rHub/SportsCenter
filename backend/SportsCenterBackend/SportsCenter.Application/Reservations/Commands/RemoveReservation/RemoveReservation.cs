using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Commands.RemoveReservation
{
    public sealed record RemoveReservation : ICommand<Unit>
    {
        public int Id { get; set; }

        public RemoveReservation(int id)
        {
            Id = id;
        }
    }
}
