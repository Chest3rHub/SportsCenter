﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ClientsExceptions
{
    public sealed class NotEnoughFundsInAccountBalance : Exception
    {     
        public NotEnoughFundsInAccountBalance() : base($"Not enough funds in account balance")
        {
        }
    }
}
