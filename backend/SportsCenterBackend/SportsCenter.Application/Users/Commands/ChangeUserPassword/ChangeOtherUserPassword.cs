using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Users.Commands.ChangeUserPassword
{
    public sealed record ChangeOtherUserPassword : ICommand<Unit>
    {
        public string Value { get; set; } = null!;
        public int UserId { get; set; }

        public ChangeOtherUserPassword(string value, int userId)
        {
            Value = value;
            UserId = userId;
        }
    }
}
