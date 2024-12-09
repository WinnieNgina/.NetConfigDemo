using DemoPractise.Records.Product;
using FluentValidation;

namespace DemoPractise.Validation;

public class ProductUpdateValidator : AbstractValidator<ProductRecord>
{
    public ProductUpdateValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product name is Required")
            .MaximumLength(100).WithMessage("Product Name can't exceed 100 characters.");
        RuleFor(x => x.ProductDescription)
            .NotEmpty().WithMessage("Product description is required")
            .MaximumLength(200).WithMessage("Product description can't exceed 200");
        
    }
}
