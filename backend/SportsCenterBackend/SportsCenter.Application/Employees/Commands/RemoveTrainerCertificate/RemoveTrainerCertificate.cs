using MediatR;
using SportsCenter.Application.Abstractions;
using SportsCenter.Application.Employees.Commands.DismissEmployee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.DeleteTrainerCertificate
{
    public sealed record RemoveTrainerCertificate : ICommand<Unit>
    {
        public int TrainerId { get; set; }
        public int CertyficateId { get; set; }       
        public RemoveTrainerCertificate(int trainerId, int certificateDate)
        {
            TrainerId = trainerId;
            CertyficateId = certificateDate;
        }
    }
}
