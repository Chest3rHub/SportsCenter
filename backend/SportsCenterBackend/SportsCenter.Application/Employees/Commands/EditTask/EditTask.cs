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
        public int ZadanieId { get; set; }
        public string Opis { get; set; } = null!;

        public DateTime DataDo { get; set; }

        public EditTask(int zadanieId, string opis, DateTime dataDo)
        {
            ZadanieId = zadanieId;
            Opis = opis;
            DataDo = dataDo;         
        }
    }
}
