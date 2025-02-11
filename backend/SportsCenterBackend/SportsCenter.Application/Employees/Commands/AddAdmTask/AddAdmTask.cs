using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.AddAdmTask
{
    public sealed record AddAdmTask : ICommand<Unit>
    {
        public string Opis { get; set; } = null!;

        public DateTime DataDo { get; set; }
        public int PracownikId { get; set; }
        
        public AddAdmTask(string opis, DateTime dataDo, int pracownikId)
        {
            Opis = opis;
            DataDo = dataDo;
            PracownikId = pracownikId;
        }
    }
}
