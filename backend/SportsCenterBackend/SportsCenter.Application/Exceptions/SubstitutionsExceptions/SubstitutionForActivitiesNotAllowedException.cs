using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.SubstitutionsExceptions
{
    public class SubstitutionForActivitiesNotAllowedException : Exception
    {
        public int Id { get; set; }

        public SubstitutionForActivitiesNotAllowedException(int id) : base($"You are not a trainer for sport activity with id: {id}")
        {
            Id = id;
        }
    }
}
