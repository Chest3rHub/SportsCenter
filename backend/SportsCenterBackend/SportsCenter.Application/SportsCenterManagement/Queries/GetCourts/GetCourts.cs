﻿using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.SportsCenterManagement.Queries.GetCourts
{
    public class GetCourts : IQuery<IEnumerable<CourtDto>>
    {
        public GetCourts()
        {
        }
    }
}
