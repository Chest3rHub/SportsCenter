using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Clients.Commands.AddDiscount
{
    internal class AddDiscountValidator : AbstractValidator<AddDiscount>
    {
        public AddDiscountValidator()
        {
            RuleFor(x => x.ActivityDiscount)
                 .GreaterThanOrEqualTo(0)
                 .WithMessage("Sports activity discount must be greater than or equal to 0");
     
            RuleFor(x => x.ProductDiscount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Products discount must be greater than or equal to 0");
        }
    }
}
