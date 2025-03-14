using SportsCenter.Application.Abstractions;
using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Activities.Queries.GetSportActivity
{
    public class GetSportActivity : IQuery<SportActivityDto>
    {
        public int SportActivityId { get; set; }
        public GetSportActivity(int sportActivityId)
        {
            SportActivityId = sportActivityId;
        }
    }
}
