using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Commands.EditProduct.EditPrice;
public record EditProductPriceCommand(Guid UserId, Guid Id, decimal Price) : IRequest;
public class EditProductPriceCommandHandler : IRequestHandler<EditProductPriceCommand>
{
    public EditProductPriceCommandHandler(IApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public IApplicationDbContext DbContext { get; }

    public async Task Handle(EditProductPriceCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        if (entity.UserID != request.UserId) throw new UnauthorizedAccessException();

        entity.AddDomainEvent(new ProductEditedEvent(entity, nameof(Product.Price)));

        entity.Price = request.Price;

        await DbContext.SaveChangesAsync(cancellationToken);
    }
}

public class EditProductPriceCommandValidator : AbstractValidator<EditProductPriceCommand>
{
    public EditProductPriceCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(c => c.UserId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(c => c.Price)
            .NotEmpty()
            .GreaterThan(0)
            .LessThan(decimal.MaxValue);
    }
}