using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.News.Commands.UpdateNews
{
    public class UpdateNewsValidator : AbstractValidator<UpdateNews>
    {
        public UpdateNewsValidator()
        {
            RuleFor(x => x.ValidFrom)
               .LessThan(x => x.ValidUntil)
               .WithMessage("The 'Valid From' date must be earlier than the 'Valid Until' date.");
        }
    }
}
