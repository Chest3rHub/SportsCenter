using MediatR;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Activities.Commands.SignUpForActivity
{
    internal class SignUpForActivityHandler : IRequestHandler<SignUpForActivity, Unit>
    {
        private readonly ISportActivityRepository _sportActivityRepository;
        private readonly ISportsCenterRepository _sportsCenterRepository;
        private readonly ICourtRepository _courtRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public SignUpForActivityHandler(ISportActivityRepository SportActivityRepository, ISportsCenterRepository sportsCenterRepository, IEmployeeRepository employeeRepository, ICourtRepository courtRepository)
        {
            _sportActivityRepository = SportActivityRepository;
            _sportsCenterRepository = sportsCenterRepository;
            _employeeRepository = employeeRepository;
            _courtRepository = courtRepository;
        }

        public async Task<Unit> Handle(SignUpForActivity request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
