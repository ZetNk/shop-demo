using FluentValidation;
using Shop.API.Models;

namespace Shop.API.Validators;

public class CartItemValidator : AbstractValidator<CartItem>
{
    public CartItemValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Please add Name");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Please add Description");
        RuleFor(x => x.Price).NotEmpty().WithMessage("Please add Price");
    }
}