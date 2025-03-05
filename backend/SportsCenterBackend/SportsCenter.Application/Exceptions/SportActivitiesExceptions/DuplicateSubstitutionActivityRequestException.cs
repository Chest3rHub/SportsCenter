using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.SportActivitiesExceptions
{
    public class DuplicateSubstitutionActivityRequestException : Exception
    {
        public int Id { get; set; }
        public DuplicateSubstitutionActivityRequestException(int id) : base($"Substitution request for activity with id: {id} has already been submitted.")
        {
            Id = id;
        }
    }
}
