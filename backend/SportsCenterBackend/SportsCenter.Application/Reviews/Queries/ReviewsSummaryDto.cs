using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reviews.Queries
{
    public class ReviewsSummaryDto
    {
        public string Description { get; set; }
        public int Stars { get; set; }
        public DateTime Date { get; set; }
        public string ClientName { get; set; }
        public string TrainerName { get; set; }     
        public string ClientSurname { get; set; }
        public string TrainerSurname { get; set; }
        public string ActivityName { get; set; } 
        public string ActivityLevel { get; set; }
    }
}
