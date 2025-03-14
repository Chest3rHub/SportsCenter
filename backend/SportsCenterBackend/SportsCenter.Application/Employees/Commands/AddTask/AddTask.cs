using MediatR;
using SportsCenter.Application.Abstractions;
using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.AddTask
{
    public sealed record AddTask : ICommand<Unit>
    {
        public string Description { get; set; } = null!;

        public DateTime DateTo { get; set; }

        public AddTask(string opis, DateTime dataDo)
        {
            Description = opis;
            DateTo = dataDo;
        }
    }
}
