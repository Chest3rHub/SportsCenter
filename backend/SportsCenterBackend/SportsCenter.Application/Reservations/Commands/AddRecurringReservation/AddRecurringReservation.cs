using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Commands.AddRecurringReservation
{
    public sealed record AddRecurringReservation : ICommand<Unit>
    {
        public int ClientId { get; set; }
        public int CourtId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreationDate { get; set; }
        public int? TrainerId { get; set; }
        public int ParticipantsCount { get; set; }
        public bool IsEquipmentReserved { get; set; }
        public string Recurrence { get; set; } 
        public DateTime RecurrenceEndDate { get; set; }
    }
}
