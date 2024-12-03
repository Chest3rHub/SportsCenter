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
        public string Opis { get; set; } = null!;

        public DateTime DataDo { get; set; }

        public int PracownikId { get; set; }

        public int PracownikZlecajacyId { get; set; }

        public AddTask(string opis, DateTime dataDo, int pracownikId, int pracownikZlecajacyId)
        {
            Opis = opis;
            DataDo = dataDo;
            PracownikId = pracownikId;
            PracownikZlecajacyId = pracownikZlecajacyId;
        }
    }
}
