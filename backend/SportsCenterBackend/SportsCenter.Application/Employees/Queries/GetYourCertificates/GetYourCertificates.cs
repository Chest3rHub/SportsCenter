﻿using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Queries.GetYourCertificates
{
    public class GetYourCertificates : IQuery<IEnumerable<YourCertificatesDto>>
    {
        public int Offset { get; set; } = 0;
        public GetYourCertificates(int offSet)
        {
            Offset = offSet;
        }
    }
}
