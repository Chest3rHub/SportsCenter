using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.SportsCenterExceptions
{
    public sealed class DayOfWeekNotFoundException : Exception
    {
        public int Id;

        public DayOfWeekNotFoundException(int id) : base($"Day with id: {id} not found")
        {
            Id = id;
        }
    }
}
