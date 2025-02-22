using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.UpdateTrainerCertificate
{
    public sealed record UpdateTrainerCertificate : ICommand<Unit>
    {
        public int TrainerId { get; set; }
        public int CertyficateId { get; set; }
        public string CertyficateName { get; set; }
        public DateOnly ReceivedDate { get; set; }
        public UpdateTrainerCertificate(int trainerId, int certificateDate, string certificateName, DateOnly receivedDate)
        {
            TrainerId = trainerId;
            CertyficateId = certificateDate;
            CertyficateName = certificateName;
            ReceivedDate = receivedDate;
        }
    }
}
