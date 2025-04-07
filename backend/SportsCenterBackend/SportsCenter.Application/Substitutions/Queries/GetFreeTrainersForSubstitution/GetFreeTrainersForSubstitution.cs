using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Substitutions.Queries.GetFreeTrainersForSubstitution
{
    public class GetFreeTrainersForSubstitution : IQuery<IEnumerable<FreeTrainerForSubstitutionDto>>
    {
        public DateTime Date {  get; set; }
        public TimeSpan StartHour { get; set; }
        public TimeSpan EndHour { get; set; }
        public int Offset { get; set; } = 0;

        public GetFreeTrainersForSubstitution(DateTime date, TimeSpan startHour, TimeSpan endHour, int offSet)
        {
            Date = date;
            StartHour = startHour;
            EndHour = endHour;
            Offset = offSet;
        }
    }
}
