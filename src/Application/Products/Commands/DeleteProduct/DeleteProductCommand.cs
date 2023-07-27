using Application.Common.Interfaces;
using Domain.Events;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Commands.DeleteProduct;
public record DeleteProductCommand(Guid UserId, Guid Id) : IRequest;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    public DeleteProductCommandHandler(IApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public IApplicationDbContext DbContext { get; }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity is null) throw new NotFoundException(nameof(entity), request.Id);
        if (entity.UserID != request.UserId) throw new UnauthorizedAccessException(nameof(entity));

        entity.AddDomainEvent(new ProductDeletedEvent(entity));

        DbContext.Products.Remove(entity);

        await DbContext.SaveChangesAsync(cancellationToken);
    }
}
