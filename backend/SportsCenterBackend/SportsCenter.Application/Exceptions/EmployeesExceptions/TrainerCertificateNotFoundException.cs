using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.EmployeesExceptions
{
    public class TrainerCertificateNotFoundException : Exception
    {
        public int CertificateId { get; set; }
        public int TrainerId { get; set; }
        public TrainerCertificateNotFoundException(int certificateId, int trainerId) : base($"Certificate with id: {certificateId} not found for trainer with id: {trainerId}")
        {
            CertificateId = certificateId;
            TrainerId = trainerId;
        }
    }
}
