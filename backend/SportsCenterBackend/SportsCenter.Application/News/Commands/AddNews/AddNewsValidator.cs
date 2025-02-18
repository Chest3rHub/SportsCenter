using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.News.Commands.AddNews
{
    public class AddNewsValidator : AbstractValidator<AddNews>
    {
        public AddNewsValidator()
        {
            RuleFor(x => x.ValidFrom)
               .LessThan(x => x.ValidUntil)
               .WithMessage("The 'Valid From' date must be earlier than the 'Valid Until' date.");
        }
    }
}
