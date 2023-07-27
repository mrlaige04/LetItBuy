using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Commands.EditProduct.EditCurrency;
public record EditProductCurrencyCommand(Guid UserId, Guid Id, string Currency) : IRequest;

public class EditProductCurrencyCommandHandler : IRequestHandler<EditProductCurrencyCommand>
{
    public EditProductCurrencyCommandHandler(IApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public IApplicationDbContext DbContext { get; }

    public async Task Handle(EditProductCurrencyCommand request, CancellationToken cancellationToken)
    {
        var entity = await DbContext.Products.FirstOrDefaultAsync(p=>p.Id == request.Id, cancellationToken) 
            ?? throw new NotFoundException(nameof(Product), request.Id);

        if (entity.UserID != request.UserId) throw new UnauthorizedAccessException();

        entity.AddDomainEvent(new ProductEditedEvent(entity, nameof(Product.Currency)));

        entity.Currency = request.Currency;

        await DbContext.SaveChangesAsync(cancellationToken);
    }
}