using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.SportActivitiesExceptions
{
    public class InstanceOfActivityNotFoundException : Exception
    {
        public int Id { get; set; }

        public InstanceOfActivityNotFoundException(int id) : base($"Instance of activity with id: {id} not found")
        {
            Id = id;
        }
    }
}
