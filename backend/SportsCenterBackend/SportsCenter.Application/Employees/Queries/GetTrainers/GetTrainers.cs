using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Queries.GetTrainers
{
    public class GetTrainers : IQuery<IEnumerable<TrainerDto>>
    {
        public int Offset { get; set; }
        public GetTrainers(int offSet)
        {
            Offset = offSet;
        }
    }
}
