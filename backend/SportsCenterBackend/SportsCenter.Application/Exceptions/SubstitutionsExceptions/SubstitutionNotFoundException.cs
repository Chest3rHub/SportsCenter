using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.SubstitutionsExceptions
{
    public class SubstitutionNotFoundException : Exception
    {
        public int Id { get; set; }

        public SubstitutionNotFoundException(int id) : base($"Substitution with id: {id} not found.")
        {
            Id = id;
        }
    }
}
