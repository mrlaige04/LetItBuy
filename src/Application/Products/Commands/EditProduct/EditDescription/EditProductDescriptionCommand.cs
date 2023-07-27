using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Commands.EditProduct.EditDescription;
public record EditProductDescriptionCommand(Guid UserId, Guid Id, string Description) : IRequest;

public class EditProductDescriptionCommandHandler : IRequestHandler<EditProductDescriptionCommand>
{
    public EditProductDescriptionCommandHandler(IApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public IApplicationDbContext DbContext { get; }

    public async Task Handle(EditProductDescriptionCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        if (entity.UserID != request.UserId) throw new UnauthorizedAccessException();

        entity.AddDomainEvent(new ProductEditedEvent(entity, nameof(Product.Description)));
        
        entity.Description = request.Description;

        await DbContext.SaveChangesAsync(cancellationToken);
    }
}

public class EditProductDescriptionCommandValidator : AbstractValidator<EditProductDescriptionCommand>
{
    public EditProductDescriptionCommandValidator()
    {
        RuleFor(p => p.Description)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(1000);
    }
}