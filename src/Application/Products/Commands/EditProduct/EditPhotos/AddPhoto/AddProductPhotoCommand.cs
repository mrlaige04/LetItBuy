using Application.Common.Interfaces;
using Application.Products.Commands.CreateProduct;
using Domain.Entities;
using Domain.Events;
using Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Application.Products.Commands.EditProduct.EditPhotos.AddPhoto;
public record AddProductPhotoCommand(Guid UserId, Guid ProductId, PhotoDTO Photo) : IRequest;
public class AddProductPhotoCommandHandler : IRequestHandler<AddProductPhotoCommand>
{
    public AddProductPhotoCommandHandler(IApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public IApplicationDbContext DbContext { get; }

    public async Task Handle(AddProductPhotoCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), request.ProductId);

        if (entity.UserID != request.UserId) throw new UnauthorizedAccessException();

        entity.AddDomainEvent(new ProductEditedEvent(entity, nameof(Product.Photos)));

        entity.Photos.Add(new ProductPhoto() { Base64Image = request.Photo.Img });

        await DbContext.SaveChangesAsync(cancellationToken);
    }
}

public partial class AddProductPhotoCommandValidator : AbstractValidator<AddProductPhotoCommand>
{
    public AddProductPhotoCommandValidator()
    {
        RuleFor(c => c.ProductId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(c => c.UserId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(c => c.Photo)
            .Must(p => p != null && Base64Regex().IsMatch(p.Img));
    }

    [GeneratedRegex(@"^data:image\/(png|jpe?g|gif);base64,(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{4})$")]
    private static partial Regex Base64Regex();
}