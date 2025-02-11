using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Clients.Commands.AddDepositYourself
{
    public sealed record AddDepositYourself : ICommand<Unit>
    {
        public decimal Deposit { get; set; }  

        public AddDepositYourself(decimal deposit)
        {
            Deposit = deposit;          
        }
    }
}
