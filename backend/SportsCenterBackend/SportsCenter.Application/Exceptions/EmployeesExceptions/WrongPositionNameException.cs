using SportsCenter.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.EmployeesException
{
    public sealed class WrongPositionNameException : CustomException
    {
        public string PositionName { get; set; }
        public WrongPositionNameException(string positionName) : base($"Position name: {positionName} does not exist.")
        {
            PositionName = positionName;
        }
    }
}
