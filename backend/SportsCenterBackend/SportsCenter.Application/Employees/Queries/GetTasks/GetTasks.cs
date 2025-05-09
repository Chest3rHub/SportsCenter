﻿using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Queries.GetTasks
{
    public class GetTasks : IQuery<IEnumerable<TaskDto>>
    {
        public int Offset { get; set; } = 0;
        public GetTasks(int offSet)
        {
            Offset = offSet;
        }
    }
}
