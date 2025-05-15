using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Queries.GetYourReservations
{
    public class GetAllReservations : IQuery<IEnumerable<AllReservationsDto>>
    {
        public int Offset { get; set; } = 0;
        public GetAllReservations(int offSet)
        {
            Offset = offSet;
        }
    }
}
