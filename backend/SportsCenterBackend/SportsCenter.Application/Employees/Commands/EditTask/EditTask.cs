using MediatR;
using SportsCenter.Application.Abstractions;
using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.EditTask
{
    public sealed record EditTask : ICommand<Unit>
    {
        public int TaskId { get; set; }
        public string Description { get; set; } = null!;
        public DateTime DateTo { get; set; }

        public EditTask(int taskId, string description, DateTime dateTo)
        {
            TaskId = taskId;
            Description = description;
            DateTo = dateTo;         
        }
    }
}
