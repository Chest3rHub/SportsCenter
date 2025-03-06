using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Substitutions.Queries.GetFreeTrainersForSubstitution
{
    public class FreeTrainerForSubstitutionDto
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
