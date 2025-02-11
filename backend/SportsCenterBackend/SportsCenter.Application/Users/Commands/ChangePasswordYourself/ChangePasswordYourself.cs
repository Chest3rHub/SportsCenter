using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Users.Commands.ChangePassowrd
{
    public sealed record ChangePasswordYourself : ICommand<Unit>
    {
        public string Value { get; set; } = null!;
 
        public ChangePasswordYourself(string value)
        {
            Value = value;
        }
    }
}
