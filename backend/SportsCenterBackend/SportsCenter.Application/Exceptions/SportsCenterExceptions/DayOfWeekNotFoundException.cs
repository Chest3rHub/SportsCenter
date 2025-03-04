using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.SportsCenterExceptions
{
    public sealed class DayOfWeekNotFoundException : Exception
    {
        public string Name;

        public DayOfWeekNotFoundException(string name) : base($"Day with name: {name} not found")
        {
            Name = name;
        }
    }
}
