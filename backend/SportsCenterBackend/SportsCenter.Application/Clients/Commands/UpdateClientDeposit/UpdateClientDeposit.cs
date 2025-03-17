using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Clients.Commands.UpdateClientDeposit
{
    public sealed record UpdateClientDeposit : ICommand<Unit>
    {
        public decimal Deposit { get; set; }
        public string Email { get; set; }

        public UpdateClientDeposit(decimal deposit, string email)
        {
            Deposit = deposit;
            Email = email;
        }
    }
}
