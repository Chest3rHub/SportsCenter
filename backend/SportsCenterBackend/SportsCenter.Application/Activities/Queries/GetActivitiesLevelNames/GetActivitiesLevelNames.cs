﻿using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Activities.Queries.GetActivitiesLevelNames
{
    public class GetActivitiesLevelNames : IQuery<IEnumerable<ActivityLevelNameDto>>
    {
        public GetActivitiesLevelNames()
        {
        }
    }
}
