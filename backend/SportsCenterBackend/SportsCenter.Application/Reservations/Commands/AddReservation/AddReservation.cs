using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Commands.AddReservation
{
    public sealed record AddReservation : ICommand<Unit>
    {
        public int ClientId { get; set; }
        public int CourtId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreationDate { get; set; }
        public int? TrainerId { get; set; }
        public int ParticipantsCount { get; set; }
        public bool IsEquipmentReserved { get; set; }
    }
}
