﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Queries.GetYourCertificates
{
    public class YourCertificatesDto
    {
        public string CertificateName { get; set; }
        public DateOnly ReceivedDate { get; set; }
    }
}
