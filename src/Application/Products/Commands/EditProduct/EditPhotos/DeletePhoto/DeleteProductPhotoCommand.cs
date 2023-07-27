using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Commands.EditProduct.EditPhotos.DeletePhoto;
public record DeleteProductPhotoCommand(Guid PhotoId, Guid UserId) : IRequest;

public class DeleteProductPhotoCommandHandler : IRequestHandler<DeleteProductPhotoCommand>
{
    public DeleteProductPhotoCommandHandler(IApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public IApplicationDbContext DbContext { get; }

    public async Task Handle(DeleteProductPhotoCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.Photos
            .Include(p=>p.Product)
            .FirstOrDefaultAsync(p => p.Id == request.PhotoId, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), request.PhotoId);

        if (entity.Product.UserID != request.UserId) throw new UnauthorizedAccessException();

        entity.AddDomainEvent(new ProductEditedEvent(entity.Product, nameof(Product.Photos)));

        DbContext.Photos.Remove(entity);

        await DbContext.SaveChangesAsync(cancellationToken);
    }
}

public class DeleteProductPhotoCommandValidator : AbstractValidator<DeleteProductPhotoCommand>
{
    public DeleteProductPhotoCommandValidator()
    {
        RuleFor(c => c.PhotoId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(c => c.UserId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}