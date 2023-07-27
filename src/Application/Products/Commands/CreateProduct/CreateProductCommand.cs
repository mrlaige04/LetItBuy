using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Commands.CreateProduct;

public record PhotoDTO (string Img);
public class CreateProductCommand : IRequest
{
    public Guid UserID { get; set; }
    public Guid CategoryID { get; init; }
    public string Title { get; init; } = null!;
    public string? Description { get; init; } 
    public decimal Price { get; init; }
    public string Currency { get; init; } = null!;
    public ICollection<PhotoDTO>? Photos { get; init; }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateProductCommand>
{
    public CreateTodoItemCommandHandler(IApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public IApplicationDbContext DbContext { get; }

    public async Task Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var user = await DbContext.AppUsers.FirstOrDefaultAsync(u => u.Id == request.UserID, cancellationToken);

        if (user is null) throw new NotFoundException(nameof(user), request.UserID);

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
                Base64Image = x.Img
            }).ToList();
        }

        entity.AddDomainEvent(new ProductCreatedEvent(entity));

        user.Products.Add(entity);

        await DbContext.SaveChangesAsync(cancellationToken);
    }
}