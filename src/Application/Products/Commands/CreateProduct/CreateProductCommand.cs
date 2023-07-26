using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Application.Products.Commands.CreateProduct;

public record PhotoDTO (string Base64Image);
public class CreateProductCommand : IRequest
{
    public Guid CategoryID { get; init; }
    public string Title { get; init; } = null!;
    public string? Description { get; init; } 
    public decimal Price { get; init; }
    public string Currency { get; init; } = null!;
    public ICollection<PhotoDTO>? Photos { get; init; }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateProductCommand>
{
    public CreateTodoItemCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUser)
    {
        DbContext = dbContext;
        CurrentUser = currentUser;
    }

    public IApplicationDbContext DbContext { get; }
    public ICurrentUserService CurrentUser { get; }

    public async Task Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var user = CurrentUser.UserID;

        if (user == Guid.Empty) throw new UnauthorizedAccessException();

        var entity = new Product()
        {
            CategoryID = request.CategoryID,
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            Currency = request.Currency
        };

        if (request.Photos != null && request.Photos.Any())
        {
            entity.Photos = request.Photos.Select(x => new ProductPhoto()
            {
                Base64Image = x.Base64Image
            }).ToList();
        }

        entity.AddDomainEvent(new ProductCreatedEvent(entity));

        DbContext.Products.Add(entity);

        await DbContext.SaveChangesAsync(cancellationToken);
    }
}