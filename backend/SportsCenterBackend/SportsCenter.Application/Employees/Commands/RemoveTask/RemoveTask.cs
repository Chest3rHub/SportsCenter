using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.RemoveTask
{
    public sealed record RemoveTask : ICommand<Unit>
    {
        public int TaskId { get; set; }

        public RemoveTask(int taskId)
        {
            TaskId = taskId;
        }
    }
}
