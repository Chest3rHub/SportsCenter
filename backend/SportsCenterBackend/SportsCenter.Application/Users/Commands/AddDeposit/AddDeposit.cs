﻿using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Users.Commands.AccountDeposit
{
    public sealed record AddDeposit : ICommand<Unit>
    {
        public decimal Deposit { get; set; }
        public string Email { get; set; }

        public AddDeposit(decimal deposit, string email) { 
            Deposit = deposit;
            Email = email;
        }

    }
}
