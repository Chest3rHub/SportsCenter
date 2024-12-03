using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.TaskExceptions
{
    public sealed class TaskNotFoundException : Exception
    {
        public int Id;

        public TaskNotFoundException(int id) : base($"Task with id: {id} not found")
        {
            Id = id;
        }
    }
}
