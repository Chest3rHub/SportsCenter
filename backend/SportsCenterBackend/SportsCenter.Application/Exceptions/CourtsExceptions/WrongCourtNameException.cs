using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.CourtsExceptions
{
    public class WrongCourtNameException : Exception
    {
        public string CourtName { get; set; }

        public WrongCourtNameException(string courtName) : base($"Court with name: {courtName} not found")
        {
            CourtName = courtName;
        }
    }
}
