using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Commands.EditProduct.EditTitle;
public record EditProductTitleCommand(Guid UserId, Guid Id, string Title) : IRequest;
public class EditProductTitleCommandHandler : IRequestHandler<EditProductTitleCommand>
{
    public EditProductTitleCommandHandler(IApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public IApplicationDbContext DbContext { get; }

    public async Task Handle(EditProductTitleCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.Products.FirstOrDefaultAsync(p => p.Id == request.Id) 
            ?? throw new NotFoundException(nameof(Product), request.Id);

        if (entity.UserID != request.UserId) throw new UnauthorizedAccessException();

        entity.AddDomainEvent(new ProductEditedEvent(entity, nameof(Product.Title)));

        entity.Title = request.Title;

        await DbContext.SaveChangesAsync(cancellationToken);
    }
}

public class EditProductTitleCommandValidator : AbstractValidator<EditProductTitleCommand>
{
    public EditProductTitleCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(255);

        RuleFor(c => c.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(c => c.UserId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}
