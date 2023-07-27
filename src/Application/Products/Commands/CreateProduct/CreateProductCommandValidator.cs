using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Products.Commands.CreateProduct
{
    public partial class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(c => c.Price)
                .NotEmpty()
                .GreaterThan(0)
                .LessThan(decimal.MaxValue);

            RuleFor(c => c.UserID)
                .NotEmpty()
                .NotEqual(Guid.Empty);

            RuleFor(c => c.CategoryID) 
                .NotEmpty()
                .NotEqual(Guid.Empty);

            RuleFor(c => c.Currency)
                .NotEmpty();

            RuleForEach(c => c.Photos)
                .Must(p => p != null && Base64Regex().IsMatch(p.Img));
            
        }

        [GeneratedRegex(@"^data:image\/(png|jpe?g|gif);base64,(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{4})$")]
        public static partial Regex Base64Regex();
    }
}
