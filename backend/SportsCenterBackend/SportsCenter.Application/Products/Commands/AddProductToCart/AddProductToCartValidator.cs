using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Products.Commands.AddProductToCart
{
    public class AddProductToCartValidator : AbstractValidator<AddProductToCart>
    {
        public AddProductToCartValidator()
        {        
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Ilość produktu musi być większa niż 0.");
        }
    }
}
