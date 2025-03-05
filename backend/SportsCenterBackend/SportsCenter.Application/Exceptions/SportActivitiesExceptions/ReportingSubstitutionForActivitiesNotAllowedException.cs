using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.SportActivitiesExceptions
{
    public class ReportingSubstitutionForActivitiesNotAllowedException : Exception
    {
        public int Id { get; set; }

        public ReportingSubstitutionForActivitiesNotAllowedException(int id) : base($"You are not a trainer for sport activity with id: {id}")
        {
            Id = id;
        }
    }
}
