using DemoPractise.Records.Product;
using FluentValidation;

namespace DemoPractise.Validation;

public class ProductCreateValidation :  AbstractValidator<CreateProductRecord>
{
    public ProductCreateValidation()
    {
        RuleFor(x => x.ProductName)
           .NotEmpty().WithMessage("Product Name is required.")
           .MaximumLength(100).WithMessage("Product Name can't exceed 100 characters.");

        RuleFor(x => x.ProductDescription)
            .NotEmpty().WithMessage("Product Description is required.")
            .MaximumLength(500).WithMessage("Product Description can't exceed 500 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");
    }
}
