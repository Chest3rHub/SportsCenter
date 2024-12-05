using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Users.Commands.ChangePassowrd
{
    public sealed record ChangePassword : ICommand<Unit>
    {
        public string Value { get; set; } = null!;

        public int UserId { get; set; }

        public ChangePassword(string value, int userId)
        {
            Value = value;
            UserId = userId;
        }
    }
}
