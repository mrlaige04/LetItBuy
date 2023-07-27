using FluentValidation;

namespace Application.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty()
                .NotEqual(Guid.Empty);

            RuleFor(c => c.Id)
                .NotEmpty()
                .NotEqual(Guid.Empty);
        }
    }
}
