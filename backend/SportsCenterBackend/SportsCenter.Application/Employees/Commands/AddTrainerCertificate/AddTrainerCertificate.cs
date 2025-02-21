using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.AddTrainerCertificate
{
    public sealed record AddTrainerCertificate : ICommand<Unit>
    {
        public int TrainerId { get; set; }
        public string CertificateName { get; set; }
        public DateOnly ReceivedDate { get; set; }

        public AddTrainerCertificate(int trainerId, string certifiateName, DateOnly receivedDate) {
            TrainerId = trainerId;
            CertificateName = certifiateName;
            ReceivedDate = receivedDate;
        }
    }
}
