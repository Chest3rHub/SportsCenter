﻿using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.News.Queries.GetNews
{
    public class GetNews : IQuery<IEnumerable<NewsDto>>
    {
    }
}
