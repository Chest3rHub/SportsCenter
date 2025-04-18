﻿using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Queries.GetYourReservations
{
    public class GetYourReservations : IQuery<IEnumerable<YourReservationDto>>
    {
        public int Offset { get; set; } = 0;
        public GetYourReservations(int offSet)
        {
            Offset = offSet;
        }
    }
}
